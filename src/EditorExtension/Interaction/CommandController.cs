using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.Execution;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Text.Editor;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public class CommandController
    {
        public List<FeatureResult> Execute(string file)
        {
            IResultsProvider resultsProvider = GetProvider(file);
            ShellController.Instance.WriteToStatusBar(resultsProvider.StatusMessage);

            string result = resultsProvider.Execute();
            List<FeatureResult> featureResults = resultsProvider.ConvertResult(result);

            ShellController.Instance.ClearStatusBar();
            return featureResults;
        }

        private IResultsProvider GetProvider(string file)
        {
            return new Cucumber(file);
        }

        public void UpdateUI(IWpfTextView wpfTextView, List<FeatureResult> featureResults)
        {
            EditorHighlighter editorHighlighter = new EditorHighlighter(wpfTextView);
            editorHighlighter.HighlightFeatureFileWithResults(featureResults);
        }
    }
}
