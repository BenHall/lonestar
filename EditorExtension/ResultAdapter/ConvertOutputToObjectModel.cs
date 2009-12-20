using System.Collections.Generic;
using System.Text.RegularExpressions;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.ResultAdapter
{
    public class ConvertOutputToObjectModel
    {
        public List<FeatureResult> Convert(string result)
        {
            string[] lines = Regex.Split(result, "\n");
            List<FeatureResult> featureResults = new List<FeatureResult>();

            FeatureResult featureResult = new FeatureResult();
            ScenarioResult scenarioResult = new ScenarioResult();

            for (int index = 0; index < lines.Length; index++)
            {
                var resultLine = lines[index].Trim();
                if (resultLine.Equals("feature_name"))
                {
                    featureResult.Name = lines[++index];
                }
                else if (resultLine.Equals("feature_done"))
                {
                    featureResults.Add(featureResult);
                }
                else if (resultLine.Equals("scenario_name"))
                {
                    scenarioResult = new ScenarioResult();
                    scenarioResult.Name = lines[++index];
                }
                else if (resultLine.Equals("after_step_result"))
                {
                    StepResult stepResult = new StepResult();
                    stepResult.Name = lines[++index];
                    stepResult.Result = lines[++index];
                    scenarioResult.StepResults.Add(stepResult);
                }
                else if (resultLine.Equals("steps_done"))
                {
                    featureResult.ScenarioResults.Add(scenarioResult);
                }
            }

            return featureResults;
        }
    }
}
