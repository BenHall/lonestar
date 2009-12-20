using System.Collections.Generic;

namespace Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels
{
    public class FeatureResult
    {
        public List<ScenarioResult> ScenarioResults { get; set; }
        public string Name { get; set; }

        public FeatureResult()
        {
            ScenarioResults = new List<ScenarioResult>();
        }
    }
}