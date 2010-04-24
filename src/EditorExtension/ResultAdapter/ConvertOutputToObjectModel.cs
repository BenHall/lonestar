using System.Collections.Generic;
using System.Text.RegularExpressions;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.ResultAdapter
{
    public class ConvertOutputToObjectModel
    {
        public List<FeatureResult> Convert(string result)
        {
            string[] lines = GetLinesFromResult(result);
            
            List<FeatureResult> featureResults = new List<FeatureResult>();
            FeatureResult featureResult = new FeatureResult();
            ScenarioResult scenarioResult = new ScenarioResult();

            for (int index = 0; index < lines.Length; index++)
            {
                var resultLine = lines[index].Trim();
                switch(resultLine)
                {
                    case("feature_name"):
                        featureResult.Name = lines[++index];
                        break;
                    case("feature_done"):
                       featureResults.Add(featureResult);
                        break;
                    case ("scenario_name"):
                        scenarioResult = new ScenarioResult {Name = lines[++index]};
                        break;
                    case("after_step_result"):
                        var stepResult = CreateStepResult(index, lines);
                        scenarioResult.StepResults.Add(stepResult);
                        break;
                    case("steps_done"):
                        featureResult.ScenarioResults.Add(scenarioResult);
                        break;
                }
            }

            return featureResults;
        }

        private string[] GetLinesFromResult(string result)
        {
            return Regex.Split(result, "\n");
        }

        private StepResult CreateStepResult(int index, string[] lines)
        {
            StepResult stepResult = new StepResult();
            stepResult.Name = lines[++index].Trim();
            stepResult.Result = lines[++index].Trim();
            return stepResult;
        }
    }
}
