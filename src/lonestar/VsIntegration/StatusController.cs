using System;
using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Meerkatalyst.Lonestar.VsIntegration
{
    public class StatusController : IServiceProvider
    {
        public LonestarPackage ServiceProvider { get; set; }
        public IVsStatusbar StatusBar { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }
        
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

        //TODO: Refactor this, feels like it's in the wrong place
        public void UpdateListsWithResults(List<FeatureResult> featureResults)
        {
            foreach (FeatureResult featureResult in featureResults)
            {
                foreach (ScenarioResult scenario in featureResult.ScenarioResults)
                {
                    foreach (StepResult stepResult in scenario.StepResults)
                    {
                        if (stepResult.ResultText == "failed")
                            AddStepToErrorList(featureResult, scenario, stepResult);
                        else if (stepResult.ResultText == "pending")
                            AddStepToTaskList(featureResult, scenario, stepResult);
                    }
                }
            }
        }

        private void AddStepToErrorList(FeatureResult featureResult, ScenarioResult scenarioResult, StepResult stepResult)
        {
            try
            {
                ErrorListProvider errorProvider = GetErrorListProvider();
                ErrorTask error = new ErrorTask();
                error.Category = TaskCategory.BuildCompile;
                error.Text = string.Format("The step \"{0}\" failed in scenario \"{1}\" for the feature \"{2}\"", stepResult.Name, scenarioResult.Name, featureResult.Name);
                errorProvider.Tasks.Add(error);
            }
            catch (InvalidOperationException)
            { }
        }

        private void AddStepToTaskList(FeatureResult featureResult, ScenarioResult scenarioResult, StepResult stepResult)
        {
            try
            {
                TaskProvider taskProvider = GetTaskListProvider();
                Task task = new Task();
                task.Category = TaskCategory.User;
                task.Text = string.Format("The step \"{0}\" is pending in scenario \"{1}\" for the feature \"{2}\"", stepResult.Name, scenarioResult.Name, featureResult.Name);
                taskProvider.Tasks.Add(task);
            }
            catch (InvalidOperationException)
            { }
        }

        public object GetService(Type serviceType)
        {
            return Package.GetGlobalService(serviceType);
        }

        //http://vsxexperience.net/2010/03/23/writing-to-the-vs-errorlist/

        private TaskProvider GetTaskListProvider()
        {
            TaskProvider provider = new TaskProvider(this);
            provider.ProviderName = "Lonestar";
            provider.ProviderGuid = new Guid("bac18fa3-71ac-4240-8008-bc12e25eab75");
            return provider;
        }

        public ErrorListProvider GetErrorListProvider()
        {
            ErrorListProvider provider = new ErrorListProvider(this);
            provider.ProviderName = "Lonestar";
            provider.ProviderGuid = new Guid("1b956816-8bbd-4ef2-ae4b-fb94a2b9adfb");
            return provider;
        }
    }
}