using System;
using System.IO;
using EnvDTE;
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
            // Show a Message Box to prove we were here
            //IVsUIShell uiShell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
            
            //Guid clsid = Guid.Empty;
            //int result;
            //Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
            //           0,
            //           ref clsid,
            //           "Meerkatalyst.Lonestar",
            //           string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this),
            //           string.Empty,
            //           0,
            //           OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //           OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
            //           OLEMSGICON.OLEMSGICON_INFO,
            //           0,        // false
            //           out result));

            IssueCommand();
        }


        //TODO: Rethink this!
        private void IssueCommand()
        {
            DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;
            string openFile = Path.Combine(dte.ActiveDocument.Path, dte.ActiveDocument.Name);
            
            CommandController controller = new CommandController();
            controller.UpdateActiveCucumberFile(openFile);
        }
    }
}
