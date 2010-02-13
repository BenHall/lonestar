using System.Windows.Media;

namespace Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers
{
    internal abstract class LineResultMarker
    {
        public abstract Brush Fill { get; set; }
        public abstract Pen Outline { get; set; }

        protected void SetupColours(Color baseColour)
        {
            Fill = new SolidColorBrush(Color.FromScRgb(100, baseColour.ScR, baseColour.ScG, baseColour.ScB));
            Fill.Freeze();
            Outline = new Pen(new SolidColorBrush(baseColour), 0.5);
            Outline.Freeze();
        }
    }
}