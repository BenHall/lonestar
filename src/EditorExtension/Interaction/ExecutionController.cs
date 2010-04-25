using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.Execution;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public class ExecutionController : StatusUpdater
    {
        public List<FeatureResult> Execute(string file)
        {
            IResultsProvider resultsProvider = GetProvider(file);
            OnUpdatedStatus(new StatusEventArgs { Message = resultsProvider.StatusMessage });

            string result = resultsProvider.Execute();
            List<FeatureResult> featureResults = resultsProvider.ConvertResult(result);

            OnUpdatedStatus(new StatusEventArgs { Clear = true });
            return featureResults;
        }

        private IResultsProvider GetProvider(string file)
        {
            return new Cucumber(file);
        }
    }
}