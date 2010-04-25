using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Meerkatalyst.Lonestar.EditorExtension.Interaction;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Meerkatalyst.Lonestar.VsIntegration
{
    public class MenuCommandController
    {
        private Package _package;
        StatusController _statusController;

        public MenuCommandController(Package package)
        {
            _package = package;
            _statusController = new StatusController
                                   {
                                       StatusBar = Package.GetGlobalService(typeof(SVsStatusbar)) as IVsStatusbar,
                                       OutputWindow = _package.GetOutputPane(VSConstants.GUID_OutWindowGeneralPane, "Lonestar"),
                                   };
        }

        public void RunLonestarOnActiveView(object sender, EventArgs e)
        {
            ActiveWindowManager activeWindowManager = new ActiveWindowManager(_package);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += ExecuteOnThread;
            worker.RunWorkerCompleted += (o, args) => UpdateUI(args.Result as List<FeatureResult>, activeWindowManager);

            worker.RunWorkerAsync(activeWindowManager.GetPathToActiveDocument());
        }

        public void RunLonestarOnSolution(object sender, EventArgs e)
        {
            ActiveWindowManager activeWindowManager = new ActiveWindowManager(_package);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += ExecuteOnThread;
            worker.RunWorkerCompleted += (o, args) => UpdateUI(args.Result as List<FeatureResult>, activeWindowManager);

            worker.RunWorkerAsync(Path.Combine(activeWindowManager.GetPathToSolution(), "features"));
        }

        private void UpdateUI(List<FeatureResult> featureResults, ActiveWindowManager activeWindowManager)
        {
            WpfUIController uiController = new WpfUIController();
            uiController.UpdatedStatus += UpdateStatus;
            uiController.UpdateUI(activeWindowManager.GetActiveView(), featureResults);
            uiController.UpdateStatusWithSummary(featureResults);
            _statusController.UpdateListsWithResults(featureResults);
        }

        private void ExecuteOnThread(object sender, DoWorkEventArgs args)
        {
            var activeDocument = args.Argument as string;

            ExecutionController executionController = new ExecutionController();
            executionController.UpdatedStatus += UpdateStatus;
            List<FeatureResult> featureResults = executionController.Execute(activeDocument);

            args.Result = featureResults;
        }

        private void UpdateStatus(object sender, StatusEventArgs e)
        {
            _statusController.WriteToStatusBar(e.Message);
            _statusController.WriteToOutputWindow(e.Summary);
        }
    }
}
