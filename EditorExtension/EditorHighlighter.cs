using System.Windows.Controls;
using System.Windows.Media;
using Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Meerkatalyst.Lonestar.EditorExtension
{
    public class EditorHighlighter
    {
        IAdornmentLayer _layer;
        IWpfTextView _view;
        
        public EditorHighlighter(IWpfTextView view)
        {
            _view = view;
            _layer = view.GetAdornmentLayer("EditorHighlighter");

            //Listen to any event that changes the layout (text changes, scrolling, etc)
            _view.LayoutChanged += OnLayoutChanged;
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                this.CreateVisuals(line);
            }
        }

        private void CreateVisuals(ITextViewLine line)
        {
            int start = line.Start;
            int end = line.End;

            for (int i = start; (i < end); ++i)
            {
                SnapshotSpan span = new SnapshotSpan(_view.TextSnapshot, Span.FromBounds(start, end));
                if (span.GetText().Trim().StartsWith("Feature"))
                {
                    HighlightLine(span, new Fail());
                }
                else if (span.GetText().Trim().StartsWith("Scenario"))
                {
                    HighlightLine(span, new Pass());
                }
                else {
                    HighlightLine(span, new Pending());
                }
            }
        }

        private void HighlightLine(SnapshotSpan span, LineResultMarker marker)
        {
            IWpfTextViewLineCollection textViewLines = _view.TextViewLines;
            Geometry g = textViewLines.GetMarkerGeometry(span);
            if (g != null)
            {
                GeometryDrawing drawing = new GeometryDrawing(marker.Fill, marker.Outline, g);
                drawing.Freeze();

                DrawingImage drawingImage = new DrawingImage(drawing);
                drawingImage.Freeze();

                Image image = new Image();
                image.Source = drawingImage;

                //Align the image with the top of the bounds of the text geometry
                Canvas.SetLeft(image, g.Bounds.Left);
                Canvas.SetTop(image, g.Bounds.Top);

                _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, image, null);
            }
        }
    }
}


