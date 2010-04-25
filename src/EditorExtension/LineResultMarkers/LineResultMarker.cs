using System.Windows.Media;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

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

        public static LineResultMarker GetResultMarker(Result result)
        {
            switch (result)
            {
                case Result.Passed:
                    return new Pass();
                case Result.Failed:
                    return new Fail();
                case Result.Skipped:
                    return new Skipped();
                case Result.Pending:
                    return new Pending();
            }

            return new Pending();
        }
    }
}