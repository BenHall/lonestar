using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Text.Editor;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public class WpfUIController : StatusUpdater
    {
        public void UpdateUI(IWpfTextView wpfTextView, List<FeatureResult> featureResults)
        {
            EditorHighlighter editorHighlighter = new EditorHighlighter(wpfTextView);
            editorHighlighter.HighlightFeatureFileWithResults(featureResults);
        }

        public void UpdateStatusWithSummary(List<FeatureResult> featureResults)
        {
            OnUpdatedStatus(new StatusEventArgs{Message = "Completed", Summary = "Done done done \r\n Done!"});
        }
    }
}