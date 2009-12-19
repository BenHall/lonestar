using System.Windows.Media;

namespace Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers
{
    class Fail : LineResultMarker
    {
        public override Brush Fill { get; set; }
        public override Pen Outline { get; set; }
        public Fail()
        {
            SetupColours(Colors.Red);
        }
    }
}


