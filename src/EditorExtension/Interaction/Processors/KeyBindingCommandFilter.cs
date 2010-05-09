using System;
using Microsoft.VisualStudio.OLE.Interop;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    internal class KeyBindingCommandFilter : IOleCommandTarget
    {
        //http://www.hill30.com/MikeFeingoldBlog/index.php/2009/09/03/django-editor-in-vs-2010-part-6-code-completion-controller/
        public IOleCommandTarget OldFilter { get; set; }

        private static readonly Guid CMDSETID_StandardCommandSet2k = new Guid("1496a755-94de-11d0-8c3f-00c04fc2aae2");
        private const uint ECMD_RETURN = 3;

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == CMDSETID_StandardCommandSet2k && nCmdID == ECMD_RETURN)
                return 0;
            
            return OldFilter.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return OldFilter.QueryStatus(pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }
    }
}