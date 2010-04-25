using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.Execution;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public class ExecutionController
    {
        public delegate void UpdatedStatusEventHandler(object sender, StatusEventArgs e);
        public event UpdatedStatusEventHandler UpdatedStatus;

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

        protected virtual void OnUpdatedStatus(StatusEventArgs e)
        {
            if (UpdatedStatus != null)
                UpdatedStatus(this, e);
        }

    }

    public class StatusEventArgs
    {
        public string Message { get; set; }
        public bool Clear { get; set; }
    }
}
