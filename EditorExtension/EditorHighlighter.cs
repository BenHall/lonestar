using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Meerkatalyst.Lonestar.EditorExtension
{
    public class EditorHighlighter
    {
        private const string RESULT_MARKER = "ResultMarker";
        IAdornmentLayer _layer;
        IWpfTextView _view;

        public EditorHighlighter(IWpfTextView view)
        {
            _view = view;
            _layer = view.GetAdornmentLayer("EditorHighlighter");
        }

        public void HighlightFeatureFileWithResults(IEnumerable<FeatureResult> featureResults)
        {
            _layer.RemoveAllAdornments();
            _layer.RemoveAdornmentsByTag(RESULT_MARKER);
            foreach (FeatureResult featureResult in featureResults)
            {
                foreach (ScenarioResult scenarioResult in featureResult.ScenarioResults)
                {
                    HighlightScenarioStepsWithResults(scenarioResult);
                }
            }
        }

        private void HighlightScenarioStepsWithResults(ScenarioResult scenarioResult)
        {
            foreach (IWpfTextViewLine line in _view.TextViewLines.WpfTextViewLines)
            {
                var lineSpan = CreateSpan(line);
                StepResult stepResult = GetResultForLine(lineSpan, scenarioResult.StepResults);
                if (stepResult != null)
                {
                    LineResultMarker resultMarker = GetResultMarker(stepResult.Result);
                    HighlightLine(lineSpan, resultMarker);
                }
            }
        }

        private StepResult GetResultForLine(SnapshotSpan line, IEnumerable<StepResult> stepResults)
        {
            return stepResults.FirstOrDefault(stepResult => line.GetText().EndsWith(stepResult.Name));
        }

        public void HighlightLine(SnapshotSpan line, LineResultMarker marker)
        {
            IWpfTextViewLineCollection textViewLines = _view.TextViewLines;
            Geometry geometry = textViewLines.GetMarkerGeometry(line);
            if (geometry != null)
            {
                Image highlight = CreateImageToHighlightLine(geometry, marker);

                Canvas.SetLeft(highlight, geometry.Bounds.Left);
                Canvas.SetTop(highlight, geometry.Bounds.Top);

                _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, line, RESULT_MARKER, highlight, null);
            }
        }

        private Image CreateImageToHighlightLine(Geometry geometry, LineResultMarker marker)
        {
            GeometryDrawing drawing = new GeometryDrawing(marker.Fill, marker.Outline, geometry);
            drawing.Freeze();

            DrawingImage drawingImage = new DrawingImage(drawing);
            drawingImage.Freeze();

            Image image = new Image();
            image.Source = drawingImage;
            return image;
        }

        private SnapshotSpan CreateSpan(ITextViewLine line)
        {
            int start = line.Start;
            int end = line.End;

            return new SnapshotSpan(_view.TextSnapshot, Span.FromBounds(start, end));
        }

        private LineResultMarker GetResultMarker(string result)
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


