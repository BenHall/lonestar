using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public class ShellController
    {
        private static ShellController _instance;
        private IVsStatusbar _statusBar;

        public static ShellController Instance
        {
            get
            {
                return _instance;
            }
        }

        public ShellController()
        {
            _instance = this;
        }

        public void SetStatusBar(IVsStatusbar vsStatusbar)
        {
            _statusBar = vsStatusbar;
        }

        public void WriteToStatusBar(string message)
        {
            int frozen;
            _statusBar.IsFrozen(out frozen);

            if (!Convert.ToBoolean(frozen))
                _statusBar.SetText(message);
        }

        public void ClearStatusBar()
        {
            int frozen;
            _statusBar.IsFrozen(out frozen);

            if (!Convert.ToBoolean(frozen))
                _statusBar.Clear();
        }
    }
}