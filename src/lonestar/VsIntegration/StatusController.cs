using System;
using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Meerkatalyst.Lonestar.VsIntegration
{
    public class StatusController
    {
        private static StatusController _instance;
        public LonestarPackage ServiceProvider { get; set; }
        public IVsStatusbar StatusBar { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        //TODO: Get rid of this approach... hacky
        public static StatusController Instance
        {
            get
            {
                return _instance;
            }
        }

        public StatusController()
        {
            _instance = this;
        }

        public void WriteToStatusBar(string message)
        {
            int frozen;
            StatusBar.IsFrozen(out frozen);

            if (!Convert.ToBoolean(frozen))
                StatusBar.SetText(message);
        }

        public void ClearStatusBar()
        {
            int frozen;
            StatusBar.IsFrozen(out frozen);

            if (!Convert.ToBoolean(frozen))
                StatusBar.Clear();
        }

        public void ClearOutputWindow()
        {
            OutputWindow.Clear();
        }

        public void WriteToOutputWindow(string summary)
        {
            OutputWindow.OutputString(summary);
            OutputWindow.Activate();
        }

        public void UpdateListsWithResults(List<FeatureResult> featureResults)
        {
            foreach (FeatureResult featureResult in featureResults)
            {
                foreach (ScenarioResult scenario in featureResult.ScenarioResults)
                {
                    foreach (StepResult stepResult in scenario.StepResults)
                    {
                        if (stepResult.Result == "failed")
                            AddStepToErrorList(stepResult);
                        else if (stepResult.Result == "pending")
                            AddStepToTaskList(stepResult);
                    }
                }
            }
        }

        private void AddStepToErrorList(StepResult stepResult)
        {
            ErrorListProvider errorProvider = new ErrorListProvider(ServiceProvider);
            ErrorTask newError = new ErrorTask();
            newError.Category = TaskCategory.Misc;
            newError.Text = "Some Error Text";
            errorProvider.Tasks.Add(newError);
        }

        private void AddStepToTaskList(StepResult stepResult)
        {
            ErrorListProvider errorProvider = new ErrorListProvider(ServiceProvider);
            Task newError = new Task();
            newError.Category = TaskCategory.Misc;
            newError.Text = "Some task Text";
            errorProvider.Tasks.Add(newError);
        }
    }
}