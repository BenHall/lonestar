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

        public static LineResultMarker GetResultMarker(string result)
        {
            switch (result)
            {
                case "passed":
                    return new Pass();
                case "failed":
                    return new Fail();
                case "skipped":
                    return new Skipped();
                case "pending":
                    return new Pending();
            }

            return new Pending();
        }
    }
}