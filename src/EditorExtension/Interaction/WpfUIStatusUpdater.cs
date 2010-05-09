using System.Collections.Generic;
using System.Text;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Text.Editor;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public class WpfUIStatusUpdater : StatusUpdateEventRaiser
    {
        public void UpdateEditor(IWpfTextView wpfTextView, List<FeatureResult> featureResults)
        {
            if (wpfTextView == null) return;
            
            EditorHighlighterProcessor editorHighlighterProcessor = new EditorHighlighterProcessor(wpfTextView);
            editorHighlighterProcessor.HighlightFeatureFileWithResults(featureResults);
        }

        public void UpdateStatusWithSummary(List<FeatureResult> featureResults)
        {
            int stepsPassed = 0;
            int stepsFailed = 0;
            int stepsPending = 0;


            StringBuilder builder = new StringBuilder();
            foreach (FeatureResult featureResult in featureResults)
            {
                builder.AppendLine("Executed " + featureResult.Name);
                foreach (ScenarioResult scenarioResult in featureResult.ScenarioResults)
                {
                    bool passed = true;
                    foreach (StepResult stepResult in scenarioResult.StepResults)
                    {
                        switch (stepResult.Result)
                        {
                            case Result.Passed:
                                stepsPassed++;
                                break;
                            case Result.Failed:
                                passed = false;
                                stepsFailed++;
                                break;
                            case Result.Pending:
                                passed = false;
                                stepsPending++;
                                break;
                        }
                    }
                    builder.AppendLine(string.Format("\tScenario: {0} {1}", scenarioResult.Name, passed ? "passed" : "failed"));
                    
                }
            }
            string message = string.Format("Execution Result {0}. Step Results - {1} passed, {2} failed, {3} pending", (stepsFailed > 0 || stepsPending > 0) ? "passed" : "failed", stepsPassed, stepsFailed, stepsPending);

            builder.AppendLine();
            builder.AppendLine(message);
            string summary = builder.ToString();
            OnUpdatedStatus(new StatusEventArgs{Message = message, Summary = summary});
        }

        public void ClearEditor(IWpfTextView wpfTextView)
        {
            if (wpfTextView == null) return;

            EditorHighlighterProcessor editorHighlighterProcessor = new EditorHighlighterProcessor(wpfTextView);
            editorHighlighterProcessor.RemoveLayer();
        }
    }
}