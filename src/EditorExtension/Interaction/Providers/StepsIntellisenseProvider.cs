using System.ComponentModel.Composition;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
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

        [Import(typeof(IVsEditorAdaptersFactoryService))]
        internal IVsEditorAdaptersFactoryService editorFactory = null;

        public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            return StepsIntellisenseProcessor.Create(wpfTextView, editorFactory);
        }
    }
}