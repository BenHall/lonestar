using System;
using Meerkatalyst.Lonestar.EditorExtension;
using Microsoft.VisualStudio.Shell;

namespace Meerkatalyst.Lonestar
{
    public class MenuCommandController
    {
        public Package Package { get; set; }

        public MenuCommandController(Package package)
        {
            Package = package;
        }

        public void MenuItemCallback(object sender, EventArgs e)
        {
            IssueCommand();
        }

        private void IssueCommand()
        {
            CommandController controller = new CommandController(); 
            ActiveWindowManager activeWindowManager = new ActiveWindowManager(Package);
            controller.RunCucumberTestsAndUpdateUI(activeWindowManager.GetPathToActiveDocument(), activeWindowManager.GetActiveView()); 
        }
    }
}
