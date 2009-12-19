using System.Windows.Media;

namespace Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers
{
    class Pending : LineResultMarker
    {
        public override Brush Fill { get; set; }
        public override Pen Outline { get; set; }
        public Pending()
        {
            SetupColours(Colors.Yellow);
        }
    }
}


