using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.Execution;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Text.Editor;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public class CommandController
    {
        public List<FeatureResult> ExecuteCucumber(string activeFilePath)
        {
            Cucumber cucumber = new Cucumber(activeFilePath);
            string result = cucumber.Execute();
            
            ConvertOutputToObjectModel converter = new ConvertOutputToObjectModel();
            return converter.Convert(result);
        }

        public void UpdateUI(IWpfTextView wpfTextView, List<FeatureResult> featureResults)
        {
            EditorHighlighter editorHighlighter = new EditorHighlighter(wpfTextView);
            editorHighlighter.HighlightFeatureFileWithResults(featureResults);
        }
    }
}
