using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.Execution;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public class ExecutionController
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
    }
}
