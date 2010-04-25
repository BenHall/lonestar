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
                            AddStepToErrorList(featureResult, scenario, stepResult);
                        else if (stepResult.Result == "pending")
                            AddStepToTaskList(featureResult, scenario, stepResult);
                    }
                }
            }
        }

        private void AddStepToErrorList(FeatureResult featureResult, ScenarioResult scenarioResult, StepResult stepResult)
        {
            ErrorListProvider errorProvider = new ErrorListProvider(ServiceProvider);
            ErrorTask error = new ErrorTask();
            error.Category = TaskCategory.BuildCompile;
            error.Text = string.Format("The step \"{0}\" failed in scenario \"{1}\" for the feature \"{2}\"", stepResult.Name, scenarioResult.Name, featureResult.Name);
            errorProvider.Tasks.Add(error);
        }

        private void AddStepToTaskList(FeatureResult featureResult, ScenarioResult scenarioResult, StepResult stepResult)
        {
            TaskProvider taskProvider = new TaskProvider(ServiceProvider);
            Task task = new Task();
            task.Category = TaskCategory.User;
            task.Text = string.Format("The step \"{0}\" is pending in scenario \"{1}\" for the feature \"{2}\"", stepResult.Name, scenarioResult.Name, featureResult.Name);
            taskProvider.Tasks.Add(task);
        }
    }
}