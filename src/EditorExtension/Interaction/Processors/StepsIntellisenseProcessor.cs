using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Meerkatalyst.Lonestar.EditorExtension.Extensions;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.IntellisenseWindow;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    public class StepsIntellisenseProcessor : KeyProcessor
    {
        private readonly IWpfTextView _view;
        private readonly IVsEditorAdaptersFactoryService _editorFactory;
        private bool justMovedCaret;
        private bool hasIntellisenseWindowOpen;
        private readonly IAdornmentLayer _layer;
        private readonly IntelliSenseControl _intellisenseWindow;
        readonly StepDefinitionFinder _finder;
        IVsTextView vsTextView;
        KeyBindingCommandFilter keyBindingCommandFilter;
        
        public StepsIntellisenseProcessor(IWpfTextView view, IVsEditorAdaptersFactoryService editorFactory)
        {
            _view = view;
            _editorFactory = editorFactory;
            _layer = view.GetAdornmentLayer(AdornmentLayerNames.StepsIntellisense);
            _intellisenseWindow = new IntelliSenseControl();
            _view.Caret.PositionChanged += ResetCaretIfIntellisenseWindowOpen;
            _view.GotAggregateFocus +=_view_GotAggregateFocus;
            _finder = new StepDefinitionFinder();
            _finder.NewStepsFound += OnNewStepsFound;
            _finder.ProcessSteps(_view.GetFilePath());    
        }

        private void _view_GotAggregateFocus(object sender, EventArgs e)
        {
            vsTextView = _editorFactory.GetViewAdapter(_view);
        }

        private void OnNewStepsFound(object sender, NewStepsFoundHandlerArgs args)
        {
            _intellisenseWindow.UpdatePopulatedView(args.NewDefinitions);
        }

        public static StepsIntellisenseProcessor Create(IWpfTextView view, IVsEditorAdaptersFactoryService editorFactory)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new StepsIntellisenseProcessor(view, editorFactory));
        }
        
        public override void KeyUp(KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Down:
                    ChangeIntellisenseSelectedItem(HighlightedSelectionAction.Down);
                    _intellisenseWindow.Focus();
                    break;
                case Key.Up:
                    ChangeIntellisenseSelectedItem(HighlightedSelectionAction.Up);
                    _intellisenseWindow.Focus();
                    break;
                case Key.Left:
                case Key.Right:
                case Key.Escape:
                    CloseIntellisenseWindow();
                    break;
                case Key.Enter:
                    _intellisenseWindow.Focus();
                    CompleteCurrentLineWithCurrentlySelectedLine();
                    break;
                default:
                    if(ShouldUpdateIntellisense(args))
                        UpdateIntellisense();
                    break;
            }

            args.Handled = true;
        }

        private bool ShouldUpdateIntellisense(KeyEventArgs args)
        {
            return args.Key >= Key.A && args.Key <= Key.Z || args.Key == Key.Back || args.Key == Key.Delete || args.Key == Key.Space;
        }

        private void OpenIntellisenseWindow()
        {
            lock (_layer)
            {
                AddCommandFilter(vsTextView);
                Canvas.SetLeft(_intellisenseWindow, 15);
                Canvas.SetTop(_intellisenseWindow, _view.Caret.Top + 15);
                _layer.Opacity = 100;
                _layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _intellisenseWindow, null);
                hasIntellisenseWindowOpen = true;
            }
        }

        private void AddCommandFilter(IVsTextView vsTextView)
        {
            IOleCommandTarget next;
            keyBindingCommandFilter = new KeyBindingCommandFilter(_view);
            vsTextView.AddCommandFilter(keyBindingCommandFilter, out next);
            keyBindingCommandFilter.OldFilter = next;
        }

        private void RemoveCommandFilter(IVsTextView vsTextView)
        {
            vsTextView.RemoveCommandFilter(keyBindingCommandFilter);
        }

        private string GetTextForLine(ITextViewLine line)
        {
            SnapshotSpan snapshotSpan = GetSnapshotSpanForLine(line);            
            return snapshotSpan.GetText();
        }

        private SnapshotSpan GetSnapshotSpanForLine(ITextViewLine line)
        {
            Span lineSpan = Span.FromBounds(line.Start, line.End);
            return new SnapshotSpan(_view.TextSnapshot, lineSpan);
        }

        private ITextViewLine GetCurrentLine()
        {
            return _view.Caret.ContainingTextViewLine;
        }

        private void UpdateIntellisense()
        {
            if (!hasIntellisenseWindowOpen)
                OpenIntellisenseWindow();

            ITextViewLine line = GetCurrentLine();
            _intellisenseWindow.HighlightItem(GetTextForLine(line), GetCurrentCaratPosition());
            _intellisenseWindow.Focus();
        }

        private int GetCurrentCaratPosition()
        {
            int position = _view.Caret.Position.BufferPosition.Position;
            ITextSnapshotLine lineNumberFromPosition =
                _view.TextSnapshot.GetLineFromLineNumber(_view.TextSnapshot.GetLineNumberFromPosition(position));
            int offset = position - lineNumberFromPosition.Start;

            return offset;
        }

        private void CompleteCurrentLineWithCurrentlySelectedLine()
        {
            if(hasIntellisenseWindowOpen)
            {
                StepDefinition currentlySelectedItem = _intellisenseWindow.GetCurrentlySelectedItem();
                MessageBox.Show(currentlySelectedItem.CleanedName);
                //TODO: Move caret to previous line
                //Replace line with selected text
                CloseIntellisenseWindow();
            }
        }

        private void ChangeIntellisenseSelectedItem(HighlightedSelectionAction selectedAction)
        {
            _intellisenseWindow.ChangeSelection(selectedAction);
        }

        private void CloseIntellisenseWindow()
        {
            lock (_layer)
            {
                _layer.RemoveAdornment(_intellisenseWindow);
                hasIntellisenseWindowOpen = false;
                RemoveCommandFilter(vsTextView);
            }
        }

        void ResetCaretIfIntellisenseWindowOpen(object sender, CaretPositionChangedEventArgs e)
        {
            CaretPosition oldPosition = e.OldPosition;

            if (!justMovedCaret && hasIntellisenseWindowOpen)
            {
                justMovedCaret = true;
                e.TextView.Caret.MoveTo(e.TextView.GetTextViewLineContainingBufferPosition(oldPosition.BufferPosition));
            }
            else
                justMovedCaret = false;
        }

    }

    internal class KeyBindingCommandFilter : IOleCommandTarget
    {
        private IWpfTextView m_textView;
        internal IOleCommandTarget m_nextTarget;
        internal bool m_added;
        internal bool m_adorned;

        public KeyBindingCommandFilter(IWpfTextView textView)
        {
            m_textView = textView;
            m_adorned = false;
        }

        //http://www.hill30.com/MikeFeingoldBlog/index.php/2009/09/03/django-editor-in-vs-2010-part-6-code-completion-controller/
        public IOleCommandTarget OldFilter { get; set; }

        private static readonly Guid CMDSETID_StandardCommandSet2k = new Guid("1496a755-94de-11d0-8c3f-00c04fc2aae2");
        private static readonly uint ECMD_RETURN = 3;

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == CMDSETID_StandardCommandSet2k && nCmdID == ECMD_RETURN)
                return 0;
            
            return OldFilter.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return OldFilter.QueryStatus(pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }
    }
}