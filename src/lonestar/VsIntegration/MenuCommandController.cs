using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
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
            List<FeatureResult> featureResults = null;

            ActiveWindowManager activeWindowManager = new ActiveWindowManager(Package);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += RunLonestarOnThread;
            worker.RunWorkerCompleted += (o, args) =>
                                             {
                                                 featureResults = args.Result as List<FeatureResult>;
                                                 WpfUIController uiController = new WpfUIController();
                                                 uiController.UpdateUI(activeWindowManager.GetActiveView(), featureResults);
                                             };
            worker.RunWorkerAsync(activeWindowManager.GetPathToActiveDocument());    
        }
        
        private void RunLonestarOnThread(object sender, DoWorkEventArgs e)
        {
            var activeDocument = e.Argument as string;

            ExecutionController executionController = new ExecutionController();
            List<FeatureResult> featureResults = executionController.Execute(activeDocument);

            e.Result = featureResults;
        }
    }
}
