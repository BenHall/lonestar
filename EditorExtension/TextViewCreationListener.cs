using System.Collections.Generic;
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
            TextViewTracker.Views.Add(textView);
        }
    }

    public static class TextViewTracker
    {
        public static List<IWpfTextView> Views { get; set; }

        static TextViewTracker()
        {
            Views = new List<IWpfTextView>();
        }
    }
}