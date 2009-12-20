using System.Collections.Generic;

namespace Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels
{
    public class ScenarioResult
    {
        public List<StepResult> StepResults { get; set; }
        public string Name { get; set; }

        public ScenarioResult()
        {
            StepResults = new List<StepResult>();
        }
    }
}