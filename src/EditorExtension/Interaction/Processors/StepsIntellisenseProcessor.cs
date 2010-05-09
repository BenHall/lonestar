using System;
using System.Linq;
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
{//TODO: Split this into correct reposabilities
    public class StepsIntellisenseProcessor : KeyProcessor
    {
        private readonly IWpfTextView _view;
        private readonly IVsEditorAdaptersFactoryService _editorFactory;
        private bool justMovedCaret;
        private bool hasIntellisenseWindowOpen;
        private readonly IAdornmentLayer _layer;
        private readonly IntelliSenseControl _intellisenseWindow;
        IVsTextView _vsTextView;
        KeyBindingCommandFilter _keyBindingCommandFilter;

        public StepsIntellisenseProcessor(IWpfTextView view, IVsEditorAdaptersFactoryService editorFactory)
        {
            _view = view;
            _editorFactory = editorFactory;
            _layer = view.GetAdornmentLayer(AdornmentLayerNames.StepsIntellisense);
            _intellisenseWindow = new IntelliSenseControl();
            _view.Caret.PositionChanged += ResetCaretIfIntellisenseWindowOpen;
            _view.GotAggregateFocus +=GetVsTextView;
            
            FindAllSteps();
        }

        public static StepsIntellisenseProcessor Create(IWpfTextView view, IVsEditorAdaptersFactoryService editorFactory)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new StepsIntellisenseProcessor(view, editorFactory));
        }

        private void FindAllSteps()
        {
            StepDefinitionFinder finder = new StepDefinitionFinder();
            finder.NewStepsFound += OnNewStepsFound;
            finder.ProcessSteps(_view.GetFilePath());
        }

        private void GetVsTextView(object sender, EventArgs e)
        {
            if (_vsTextView == null)
                _vsTextView = _editorFactory.GetViewAdapter(_view);
        }

        private void OnNewStepsFound(object sender, NewStepsFoundHandlerArgs args)
        {
            IOrderedEnumerable<StepDefinition> stepDefinitions = args.NewDefinitions.OrderBy(x=>x.CleanedName, new GwtStringComparer());
            _intellisenseWindow.UpdatePopulatedView(stepDefinitions.ToList());
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
                AddKeyBindingFilter(_vsTextView);
                Canvas.SetLeft(_intellisenseWindow, 15);
                Canvas.SetTop(_intellisenseWindow, _view.Caret.Top + 15);
                _layer.Opacity = 100;
                _layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _intellisenseWindow, null);
                hasIntellisenseWindowOpen = true;
            }
        }

        private void AddKeyBindingFilter(IVsTextView vsTextView)
        {
            IOleCommandTarget next;
            _keyBindingCommandFilter = new KeyBindingCommandFilter();
            vsTextView.AddCommandFilter(_keyBindingCommandFilter, out next);
            _keyBindingCommandFilter.OldFilter = next;
        }

        private void RemoveKeyBindingFilter(IVsTextView vsTextView)
        {
            vsTextView.RemoveCommandFilter(_keyBindingCommandFilter);
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
                if (currentlySelectedItem != null)
                    ReplaceCurrentLineWithStep(currentlySelectedItem);
                else
                    InsertBlankLine();

                CloseIntellisenseWindow();
            }
        }

        private void InsertBlankLine()
        {
            var textBuffer = _view.TextBuffer;
            textBuffer.Insert(_view.Caret.Position.BufferPosition, "\r\n");
        }

        private void ReplaceCurrentLineWithStep(StepDefinition currentlySelectedItem)
        {
            var textBuffer = _view.TextBuffer;

            ITextViewLine line = GetCurrentLine();
            Span lineSpan = Span.FromBounds(line.Start, line.End);

            textBuffer.Delete(lineSpan);
            textBuffer.Insert(line.Start, "   " + currentlySelectedItem.CleanedName);
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
                RemoveKeyBindingFilter(_vsTextView);
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
}