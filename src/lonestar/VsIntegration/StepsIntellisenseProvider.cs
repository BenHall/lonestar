using System.ComponentModel.Composition;
using Meerkatalyst.Lonestar.EditorExtension.Interaction;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Meerkatalyst.Lonestar.VsIntegration
{
    [Export(typeof(IKeyProcessorProvider))]
    [Name("Steps Intellisense Processor")]
    [Order(Before = "default")]
    [ContentType("text")]
    [FileExtension(".feature")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class StepsIntellisenseProvider : IKeyProcessorProvider
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name(AdornmentLayerNames.StepsIntellisense)]
        [Order(After = PredefinedAdornmentLayers.Selection)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        private AdornmentLayerDefinition editorAdornmentLayer;

        public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            return StepsIntellisenseProcessor.Create(wpfTextView);
        }
    }
}