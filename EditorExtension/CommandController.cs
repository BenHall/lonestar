using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.Execution;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension
{
    public class CommandController
    {
        public void UpdateActiveCucumberFile(string activeFilePath)
        {
            TextViewTracker.Frozen = true;
            EditorHighlighter editorHighlighter = new EditorHighlighter(TextViewTracker.View);

            Cucumber cucumber = new Cucumber(activeFilePath);
            string result = cucumber.Execute();
            
            ConvertOutputToObjectModel converter = new ConvertOutputToObjectModel();
            List<FeatureResult> featureResults = converter.Convert(result);

            editorHighlighter.UpdateUI(featureResults);
        }
    }
}
