using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace Meerkatalyst.Lonestar
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(MyToolWindow))]
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

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                CreateMenuCommands(mcs);
                CreateToolWindows(mcs);
            }
        }

        private void CreateMenuCommands(OleMenuCommandService mcs)
        {
            CommandID menuCommandID = new CommandID(GuidList.guidLonestarCmdSet, (int)PkgCmdIDList.runLonestar);
            MenuCommandController controller = new MenuCommandController(this);
            MenuCommand menuItem = new MenuCommand(controller.MenuItemCallback, menuCommandID );
            mcs.AddCommand( menuItem );
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
