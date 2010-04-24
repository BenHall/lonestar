using System.Windows.Media;

namespace Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers
{
    public abstract class LineResultMarker
    {
        public Brush Fill { get; set; }
        public Pen Outline { get; set; }

        public LineResultMarker(Color baseColour)
        {
            Fill = new SolidColorBrush(Color.FromScRgb(100, baseColour.ScR, baseColour.ScG, baseColour.ScB));
            Fill.Opacity = 0.5;
            Fill.Freeze();
            Outline = new Pen(new SolidColorBrush(baseColour), 0.5);
            Outline.Freeze();
        }
    }
}