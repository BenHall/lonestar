using System;
using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension;
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
            CommandController controller = new CommandController(); 
            ActiveWindowManager activeWindowManager = new ActiveWindowManager(Package);
            List<FeatureResult> featureResults = controller.ExecuteCucumber(activeWindowManager.GetPathToActiveDocument());
            controller.UpdateUI(activeWindowManager.GetActiveView(), featureResults);
        }
    }
}
