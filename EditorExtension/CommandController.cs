using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.Execution;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter;

namespace Meerkatalyst.Lonestar.EditorExtension
{
    public class CommandController
    {
        public void UpdateActiveCucumberFile(string activeFilePath)
        {
            Cucumber cucumber = new Cucumber(activeFilePath);
            string result = cucumber.Execute();
            
            ConvertOutputToObjectModel converter = new ConvertOutputToObjectModel();
            List<FeatureResult> featureResults = converter.Convert(result);

            EditorHighlighter editorHighlighter = new EditorHighlighter(TextViewTracker.Views[0]);
            editorHighlighter.UpdateUI(featureResults);
        }
    }
}
