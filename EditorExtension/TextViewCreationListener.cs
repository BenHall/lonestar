using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Meerkatalyst.Lonestar.EditorExtension
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class TextViewCreationListener : IWpfTextViewCreationListener
    {
        public void TextViewCreated(IWpfTextView textView)
        {
            TextViewTracker.View = textView;
        }
    }

    public static class TextViewTracker
    {
        public static IWpfTextView View { get; set; }
    }
}