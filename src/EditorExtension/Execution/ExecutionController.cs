using System;
using System.Collections.Generic;
using System.IO;
using Meerkatalyst.Lonestar.EditorExtension.Interaction;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.Execution
{
    public class ExecutionController : StatusUpdateEventRaiser
    {
        public List<FeatureResult> Execute(string file)
        {
            if(!PathExists(file))
                throw new FileNotFoundException("File was not found", file);

            if (!Supported(file))
                throw new NotSupportedException(string.Format("Sorry for this path {0} is not supported", file));
            
            IResultsProvider resultsProvider = GetProvider(file);
            OnUpdatedStatus(new StatusEventArgs { Message = resultsProvider.StatusMessage });

            string result = resultsProvider.Execute();
            List<FeatureResult> featureResults = resultsProvider.ConvertResult(result);

            OnUpdatedStatus(new StatusEventArgs { Clear = true });
            return featureResults;
        }

        private bool Supported(string file)
        {
            bool supported = true;
            if (!IsDirectory(file))
                supported = Path.GetExtension(file).EndsWith("feature");

            return supported;
        }

        private bool PathExists(string path)
        {
            return IsDirectory(path) ? Directory.Exists(path) : File.Exists(path);
        }

        private bool IsDirectory(string path)
        {
            FileAttributes pathAttributes = File.GetAttributes(path);
            return (pathAttributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        private IResultsProvider GetProvider(string file)
        {
            return new Cucumber(file);
        }
    }
}