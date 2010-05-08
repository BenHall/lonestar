using System.IO;
using System.Reflection;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Meerkatalyst.Lonestar.VsIntegration
{
    public class ActiveWindowManager
    {
        public Package Package { get; set; }

        public ActiveWindowManager(Package package)
        {
            Package = package;
        }

        public string GetPathToActiveDocument()
        {
            DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;

            if (dte == null)
                return string.Empty;

            return Path.Combine(dte.ActiveDocument.Path, dte.ActiveDocument.Name);
        }

        public IWpfTextView GetActiveView()
        {
            IVsTextView currentTextView;
            IVsTextManager textManager = Package.GetGlobalService(typeof(VsTextManagerClass)) as IVsTextManager;

            if (textManager == null)
                return null;
                
            textManager.GetActiveView(0, null, out currentTextView);

            if(currentTextView == null)
                return null;

            return GetWpfView(currentTextView) as IWpfTextView;
        }

        private static object GetWpfView(IVsTextView currentTextView)
        {
            PropertyInfo property = currentTextView.GetType().GetProperty("WpfTextView");
            if (property == null)
                return null;

            return property.GetValue(currentTextView, null);
        }

        public string GetPathToSolution()
        {
            DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;
            if (dte == null)
                return string.Empty;

            return Path.GetDirectoryName(dte.Solution.FileName);
        }
    }
}
