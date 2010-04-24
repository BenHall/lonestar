using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Meerkatalyst.Lonestar
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

            return Path.Combine(dte.ActiveDocument.Path, dte.ActiveDocument.Name);
        }

        public IWpfTextView GetActiveView()
        {
            IVsTextView currentTextView;
            IVsTextManager textManager = Package.GetGlobalService(typeof(VsTextManagerClass)) as IVsTextManager;

            if (textManager == null)
                return null;
                
            textManager.GetActiveView(0, null, out currentTextView);

            return GetWpfView(currentTextView) as IWpfTextView;
        }

        private static object GetWpfView(IVsTextView currentTextView)
        {
            return currentTextView.GetType().GetProperty("WpfTextView").GetValue(currentTextView, null);
        }
    }
}
