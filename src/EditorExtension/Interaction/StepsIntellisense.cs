using System.Diagnostics;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public sealed class StepsIntellisense : VsTextViewInteractions
    {
        protected override IWpfTextView View { get; set; }
        protected override IAdornmentLayer Layer { get; set; }

        public StepsIntellisense(IWpfTextView view)
        {
            View = view;
            Layer = view.GetAdornmentLayer("StepsIntellisense");

            View.LayoutChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                UpdateIntellisense(line);
            }
        }

        private void UpdateIntellisense(ITextViewLine line)
        {
            Debug.WriteLine(line);
        }
    }
}


