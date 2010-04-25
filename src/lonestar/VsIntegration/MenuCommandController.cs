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
                                                 uiController.UpdatedStatus += UpdateStatus;
                                                 uiController.UpdateUI(activeWindowManager.GetActiveView(), featureResults);
                                                 uiController.UpdateStatusWithSummary(featureResults);
                                                 StatusController.Instance.UpdateListsWithResults(featureResults);
                                             };

            worker.RunWorkerAsync(activeWindowManager.GetPathToActiveDocument());    
        }
        
        private static void ExecuteOnThread(object sender, DoWorkEventArgs e)
        {
            var activeDocument = e.Argument as string;

            ExecutionController executionController = new ExecutionController();
            executionController.UpdatedStatus += UpdateStatus;
            List<FeatureResult> featureResults = executionController.Execute(activeDocument);

            e.Result = featureResults;
        }

        private static void UpdateStatus(object sender, StatusEventArgs e)
        {
            StatusController.Instance.WriteToStatusBar(e.Message);
            StatusController.Instance.WriteToOutputWindow(e.Summary);
        }
    }
}
