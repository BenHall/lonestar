using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Meerkatalyst.Lonestar.Properties;
using Meerkatalyst.Lonestar.VsIntegration;
using Microsoft.VisualStudio;
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

            new StatusController
                {
                    StatusBar = GetService(typeof (SVsStatusbar)) as IVsStatusbar,
                    OutputWindow = GetOutputPane(VSConstants.GUID_OutWindowGeneralPane, "Lonestar"),
                    ServiceProvider = this
                };

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                CreateMenuCommands(mcs);
                CreateToolWindows(mcs);
            }
        }

        private void CreateMenuCommands(OleMenuCommandService mcs)
        {
            CommandID runLonestarOnActiveViewMenuCommandID = new CommandID(GuidList.guidLonestarCmdSet, (int)PkgCmdIDList.runLonestarOnActiveView);
            CommandID runLonestarOnSolutionMenuCommandID = new CommandID(GuidList.guidLonestarCmdSet, (int)PkgCmdIDList.runLonestarOnSolution);
            MenuCommandController controller = new MenuCommandController(this);
            MenuCommand menuItem = new MenuCommand(controller.RunLonestarOnActiveView, runLonestarOnActiveViewMenuCommandID);
            mcs.AddCommand(menuItem);

            MenuCommand runOnSolution = new MenuCommand(controller.RunLonestarOnSolution, runLonestarOnSolutionMenuCommandID);
            mcs.AddCommand(runOnSolution);
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
