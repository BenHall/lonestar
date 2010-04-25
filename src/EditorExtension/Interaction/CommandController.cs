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
            string result = resultsProvider.Execute();

            return resultsProvider.ConvertResult(result);
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
