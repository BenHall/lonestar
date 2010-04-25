using System;
using System.Collections.Generic;
using System.ComponentModel;
using Meerkatalyst.Lonestar.EditorExtension.Interaction;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Shell;

namespace Meerkatalyst.Lonestar.VsIntegration
{
    public class MenuCommandController
    {
        public Package Package { get; set; }

        public MenuCommandController(Package package)
        {
            Package = package;
        }

        public void RunLonestarOnActiveView(object sender, EventArgs e)
        {
            ActiveWindowManager activeWindowManager = new ActiveWindowManager(Package);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += ExecuteOnThread;
            worker.RunWorkerCompleted += (o, args) =>
                                             {
                                                 var featureResults = args.Result as List<FeatureResult>;
                                                 WpfUIController uiController = new WpfUIController();
                                                 uiController.UpdateUI(activeWindowManager.GetActiveView(), featureResults);
                                             };

            worker.RunWorkerAsync(activeWindowManager.GetPathToActiveDocument());    
        }
        
        private static void ExecuteOnThread(object sender, DoWorkEventArgs e)
        {
            var activeDocument = e.Argument as string;

            ExecutionController executionController = new ExecutionController();
            executionController.UpdatedStatus += UpdateStatusBar;
            List<FeatureResult> featureResults = executionController.Execute(activeDocument);

            e.Result = featureResults;
        }

        private static void UpdateStatusBar(object sender, StatusEventArgs e)
        {
            if(e.Clear)
                ShellController.Instance.ClearStatusBar();
            else
                ShellController.Instance.WriteToStatusBar(e.Message);
        }
    }
}
