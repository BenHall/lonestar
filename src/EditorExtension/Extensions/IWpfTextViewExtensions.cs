using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Meerkatalyst.Lonestar.EditorExtension.Extensions
{
    public static class IWpfTextViewExtensions
    {
        public static string GetFilePath(this IWpfTextView wpfTextView)
        {
            ITextDocument document = GetDocumentFromBuffer(wpfTextView);

            if (wpfTextView == null || document == null || document.TextBuffer == null)
                return String.Empty;

            return document.FilePath;
        }

        private static ITextDocument GetDocumentFromBuffer(IWpfTextView wpfTextView)
        {
            ITextDocument document;
            bool failedToGet = !wpfTextView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out document);
            return failedToGet ? null : document;
        }
    }
}