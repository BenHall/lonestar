using System.Collections.Generic;
using System.Text.RegularExpressions;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.ResultAdapter
{
    public class ConvertOutputToObjectModel
    {
        private const string FEATURE_STARTED = "feature_name";
        private const string FEATURE_COMPLETED = "feature_done";
        private const string SCENARIO_STARTED = "scenario_name";
        private const string STEP_RESULT = "after_step_result";
        private const string SCENARIO_DONE = "steps_done";

        public List<FeatureResult> Convert(string rawOutput)
        {
            string[] results = GetLinesFromOutput(rawOutput);

            List<FeatureResult> featureResults = new List<FeatureResult>();
            FeatureResult featureResult = null;
            ScenarioResult scenarioResult = null;

            for (int index = 0; index < results.Length; index++)
            {
                var resultLine = results[index].Trim();
                switch (resultLine)
                {
                    case (FEATURE_STARTED):
                        featureResult = new FeatureResult { Name = results[++index] };
                        break;
                    case (FEATURE_COMPLETED):
                        featureResults.Add(featureResult);
                        break;
                    case (SCENARIO_STARTED):
                        scenarioResult = new ScenarioResult { Name = results[++index] };
                        break;
                    case (STEP_RESULT):
                        var stepResult = CreateStepResult(index, results);
                        scenarioResult.StepResults.Add(stepResult);
                        break;
                    case (SCENARIO_DONE):
                        featureResult.ScenarioResults.Add(scenarioResult);
                        break;
                }
            }

            return featureResults;
        }

        private string[] GetLinesFromOutput(string result)
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
