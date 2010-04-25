using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public abstract class VsTextViewInteractions
    {
        protected abstract IWpfTextView View { get; set; }
        protected abstract IAdornmentLayer Layer { get; set; }

        protected SnapshotSpan CreateSnapshotSpanForCurrentLine(ITextViewLine line)
        {
            Span lineSpan = Span.FromBounds(line.Start, line.End);
            return new SnapshotSpan(View.TextSnapshot, lineSpan);
        }
    }
}