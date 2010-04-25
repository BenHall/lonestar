using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Meerkatalyst.Lonestar.EditorExtension.Interaction;
using Meerkatalyst.Lonestar.Properties;
using Meerkatalyst.Lonestar.VsIntegration;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Meerkatalyst.Lonestar
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(VsIntegration.ResultsWindow))]
    [Guid(GuidList.guidLonestarPkgString)]
    public sealed class LonestarPackage : Package
    {
        public LonestarPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
        }

        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
            base.Initialize();


            ShellController controller = new ShellController();
            controller.SetStatusBar(GetService(typeof (SVsStatusbar)) as IVsStatusbar);
            
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                CreateMenuCommands(mcs);
                CreateToolWindows(mcs);
            }
        }

        private void CreateMenuCommands(OleMenuCommandService mcs)
        {
            CommandID runLonestarMenuCommandID = new CommandID(GuidList.guidLonestarCmdSet, (int)PkgCmdIDList.runLonestar);
            MenuCommandController controller = new MenuCommandController(this);
            MenuCommand menuItem = new MenuCommand(controller.RunLonestarOnActiveView, runLonestarMenuCommandID);
            mcs.AddCommand(menuItem);
        }

        private void CreateToolWindows(OleMenuCommandService mcs)
        {
            WindowLauncher launcher = new WindowLauncher(this);
            CommandID toolwndCommandID = new CommandID(GuidList.guidLonestarCmdSet, (int)PkgCmdIDList.resultsWindow);
            MenuCommand menuToolWin = new MenuCommand(launcher.ShowToolWindow, toolwndCommandID);
            mcs.AddCommand( menuToolWin );
        }
    }
}
