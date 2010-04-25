using System.ComponentModel.Composition;
using Meerkatalyst.Lonestar.EditorExtension.Interaction;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Meerkatalyst.Lonestar.VsIntegration
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [FileExtension(".feature")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class IntellisenseViewCreationListener : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("StepsIntellisense")]
        [Order(After = PredefinedAdornmentLayers.Selection)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        private AdornmentLayerDefinition editorAdornmentLayer;

        public void TextViewCreated(IWpfTextView textView)
        {
            new StepsIntellisense(textView);
        }
    }
}