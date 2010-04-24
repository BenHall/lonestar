// Guids.cs
// MUST match guids.h
using System;

namespace Meerkatalyst.Lonestar.Properties
{
    static class GuidList
    {
        public const string guidLonestarPkgString = "3535861c-f434-4192-bb72-11c9aa2eaeef";
        public const string guidLonestarCmdSetString = "1d970849-b148-442a-91ef-b8baa4f6cf18";
        public const string guidToolWindowPersistanceString = "ec204ef7-1bf9-4239-a253-9dd8490259d4";

        public static readonly Guid guidLonestarCmdSet = new Guid(guidLonestarCmdSetString);
    };
}