using System.Windows.Media;

namespace Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers
{
    class Pass : LineResultMarker
    {
        public override Brush Fill { get; set; }
        public override Pen Outline { get; set; }
        public Pass()
        {
            SetupColours(Colors.LightGreen);
        }
    }
}


