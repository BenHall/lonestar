using System.ComponentModel.Composition;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Providers
{
    [Export(typeof(IKeyProcessorProvider))]
    [Name("Steps Intellisense Processor")]
    [ContentType("text")]
    [FileExtension(".feature")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class StepsIntellisenseProvider : IKeyProcessorProvider
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name(AdornmentLayerNames.StepsIntellisense)]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        private AdornmentLayerDefinition editorAdornmentLayer;

        public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            return StepsIntellisenseProcessor.Create(wpfTextView);
        }
    }
}