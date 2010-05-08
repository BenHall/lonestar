using System.Windows.Controls;
using System.Windows.Input;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.IntellisenseWindow;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    public class StepsIntellisenseProcessor : KeyProcessor
    {
        private readonly IWpfTextView _view;
        private bool justMovedCaret;
        private bool hasIntellisenseWindowOpen;
        private readonly IAdornmentLayer _layer;
        private readonly IntelliSenseControl _intellisenseWindow;

        public StepsIntellisenseProcessor(IWpfTextView view)
        {
            _view = view;
            _layer = view.GetAdornmentLayer(AdornmentLayerNames.StepsIntellisense);
            _intellisenseWindow = new IntelliSenseControl();
            _view.Caret.PositionChanged += ResetCaretIfIntellisenseWindowOpen;
        }

        public static StepsIntellisenseProcessor Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new StepsIntellisenseProcessor(view));
        }

        public override void KeyDown(KeyEventArgs args)
        {
            if (!hasIntellisenseWindowOpen)
                ShowIntellisenseWindow();

            ITextViewLine line = GetCurrentLine();
            _intellisenseWindow.HighlightItem(GetTextForLine(line));
            base.KeyDown(args);
        }

        private void ShowIntellisenseWindow()
        {
            Canvas.SetLeft(_intellisenseWindow, _view.ViewportRight - 300);
            Canvas.SetTop(_intellisenseWindow, _view.ViewportTop + 300);
            _layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null,null, _intellisenseWindow, null);
            hasIntellisenseWindowOpen = true;
        }

        private string GetTextForLine(ITextViewLine line)
        {
            Span lineSpan = Span.FromBounds(line.Start, line.End);
            var snapshotSpan = new SnapshotSpan(_view.TextSnapshot, lineSpan);
            return snapshotSpan.GetText();
        }

        private ITextViewLine GetCurrentLine()
        {
            return _view.Caret.ContainingTextViewLine;
        }

        public override void KeyUp(KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Down:
                    ChangeIntellisenseSelection(HighlightedSelection.Down);
                    break;
                case Key.Up:
                    ChangeIntellisenseSelection(HighlightedSelection.Up);
                    break;
                case Key.Escape:
                    CloseIntellisenseWindow();
                    break;
            }

            args.Handled = true;
        }

        private void ChangeIntellisenseSelection(HighlightedSelection selectedAction)
        {
            _intellisenseWindow.ChangeSelection(selectedAction);
        }

        private void CloseIntellisenseWindow()
        {
            _layer.RemoveAdornment(_intellisenseWindow);
            hasIntellisenseWindowOpen = false;
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


