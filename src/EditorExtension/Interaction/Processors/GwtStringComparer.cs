using System.Collections.Generic;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    internal class GwtStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int xVal = GetValue(x);
            int yVal = GetValue(y);

            return xVal.CompareTo(yVal);
        }

        private int GetValue(string s)
        {
            if (s.StartsWith("G"))
                return 1;
            if (s.StartsWith("W"))
                return 2;
            if (s.StartsWith("T"))
                return 3;

            return 0;
        }
    }
}