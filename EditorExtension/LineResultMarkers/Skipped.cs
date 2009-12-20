using System.Windows.Media;

namespace Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers
{
    class Skipped : LineResultMarker
    {
        public override Brush Fill { get; set; }
        public override Pen Outline { get; set; }
        public Skipped()
        {
            SetupColours(Colors.SkyBlue);
        }
    }
}


