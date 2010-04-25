using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.Execution
{
    public interface IResultsProvider
    {
        string Execute();
        List<FeatureResult> ConvertResult(string result);
        string StatusMessage { get; }
    }
}