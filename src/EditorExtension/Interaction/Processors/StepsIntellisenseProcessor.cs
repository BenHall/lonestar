using System.Windows.Controls;
using System.Windows.Input;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.IntellisenseWindow;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows;
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
            _view.Caret.PositionChanged += Caret_PositionChanged;
        }

        public static StepsIntellisenseProcessor Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new StepsIntellisenseProcessor(view));
        }

        protected SnapshotSpan CreateSnapshotSpanForCurrentLine(ITextViewLine line)
        {
            Span lineSpan = Span.FromBounds(line.Start, line.End);
            return new SnapshotSpan(_view.TextSnapshot, lineSpan);
        }

        public override void KeyDown(KeyEventArgs args)
        {
            if (!hasIntellisenseWindowOpen)
            {
                Canvas.SetLeft(_intellisenseWindow, _view.ViewportRight - 300);
                Canvas.SetTop(_intellisenseWindow, _view.ViewportTop + 300);
                _layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null,null, _intellisenseWindow, null);
                hasIntellisenseWindowOpen = true;
            }

            ITextViewLine line = _view.Caret.ContainingTextViewLine;
            _intellisenseWindow.HighlightItem(CreateSnapshotSpanForCurrentLine(line).GetText());
            base.KeyDown(args);
        }

        public override void KeyUp(KeyEventArgs args)
        {
            if (args.Key == Key.Down)
            {
                MessageBox.Show("Down");
            }
            if (args.Key == Key.Escape)
            {
                _layer.RemoveAdornment(_intellisenseWindow);
                hasIntellisenseWindowOpen = false;
                MessageBox.Show("Escape");
            }

            args.Handled = true;
        }

        void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
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


