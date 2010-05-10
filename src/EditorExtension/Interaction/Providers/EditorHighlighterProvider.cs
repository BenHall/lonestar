using System.ComponentModel.Composition;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Providers
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [FileExtension(".feature")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class EditorHighlighterProvider : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name(AdornmentLayerNames.EditorHighlighter)]
        [Order(Before = PredefinedAdornmentLayers.Selection)]
        [TextViewRole(PredefinedTextViewRoles.Document)] 
        public AdornmentLayerDefinition EditorAdornmentLayer;

        public void TextViewCreated(IWpfTextView textView)
        {
            new EditorHighlighterProcessor(textView);
        }
    }
}