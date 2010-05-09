using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Meerkatalyst.Lonestar.EditorExtension.Execution;
using Meerkatalyst.Lonestar.EditorExtension.Interaction;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows.Forms;

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
            try
            {
                ActiveWindowManager activeWindowManager = new ActiveWindowManager(_package);
                string path = activeWindowManager.GetPathToActiveDocument();

                ClearUI(activeWindowManager);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, args) => args.Result = ExecuteOnThread(path, activeWindowManager);
                worker.RunWorkerCompleted += (o, args) => UpdateUI(args.Result as List<FeatureResult>, activeWindowManager);

                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public void RunLonestarOnSolution(object sender, EventArgs e)
        {
            try
            {
                ActiveWindowManager activeWindowManager = new ActiveWindowManager(_package);
                string path = Path.Combine(activeWindowManager.GetPathToSolution(), "features");

                ClearUI(activeWindowManager);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s,args) => args.Result = ExecuteOnThread(path, activeWindowManager);
                worker.RunWorkerCompleted += (o, args) => UpdateUI(args.Result as List<FeatureResult>, activeWindowManager);

                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void ClearUI(ActiveWindowManager activeWindowManager)
        {
            WpfUIStatusUpdater uiStatusUpdater = new WpfUIStatusUpdater();
            uiStatusUpdater.ClearEditor(activeWindowManager.GetActiveView());
        }

        private void UpdateUI(List<FeatureResult> featureResults, ActiveWindowManager activeWindowManager)
        {
            WpfUIStatusUpdater uiStatusUpdater = new WpfUIStatusUpdater();
            uiStatusUpdater.UpdatedStatus += UpdateStatus;
            uiStatusUpdater.UpdateEditor(activeWindowManager.GetActiveView(), featureResults);
            uiStatusUpdater.UpdateStatusWithSummary(featureResults);
            _statusController.UpdateListsWithResults(featureResults);
        }

        private List<FeatureResult> ExecuteOnThread(string activeDocument, ActiveWindowManager activeWindowManager)
        {
            ExecutionController executionController = new ExecutionController();
            executionController.UpdatedStatus += UpdateStatus;
            List<FeatureResult> featureResults = executionController.Execute(activeDocument);

            return featureResults;
        }

        private void UpdateStatus(object sender, StatusEventArgs e)
        {
            _statusController.WriteToStatusBar(e.Message);
            _statusController.WriteToOutputWindow(e.Summary);
        }
    }
}
