using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public class EditorHighlighter
    {
        private const string RESULT_MARKER_LAYER = "ResultMarker";
        IAdornmentLayer _layer;
        IWpfTextView _view;

        public EditorHighlighter(IWpfTextView view)
        {
            _view = view;
            _layer = view.GetAdornmentLayer("EditorHighlighter");
        }

        public void HighlightFeatureFileWithResults(IEnumerable<FeatureResult> featureResults)
        {
            RemoveExistingLayers();
            foreach (FeatureResult featureResult in featureResults)
            {
                foreach (ScenarioResult scenarioResult in featureResult.ScenarioResults)
                {
                    HighlightUIWithResults(scenarioResult);
                }
            }
        }

        private void RemoveExistingLayers()
        {
            _layer.RemoveAllAdornments();
            _layer.RemoveAdornmentsByTag(RESULT_MARKER_LAYER);
        }

        private void HighlightUIWithResults(ScenarioResult scenarioResult)
        {
            foreach (IWpfTextViewLine uiLine in _view.TextViewLines.WpfTextViewLines)
            {
                var currentLine = CreateSnapshotSpanForCurrentLine(uiLine);
                StepResult stepResult = GetResultForLine(currentLine, scenarioResult.StepResults);
                if (stepResult != null)
                {
                    LineResultMarker resultMarker = LineResultMarker.GetResultMarker(stepResult.Result);
                    HighlightLine(currentLine, resultMarker);
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

                _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, line, RESULT_MARKER_LAYER, highlight, null);
            }
        }

        private Image CreateImageToHighlightLine(Geometry geometry, LineResultMarker marker)
        {
            GeometryDrawing backgroundGeometry = new GeometryDrawing(marker.Fill, marker.Outline, geometry);
            backgroundGeometry.Freeze();

            DrawingImage backgroundDrawning = new DrawingImage(backgroundGeometry);
            backgroundDrawning.Freeze();

            return new Image {Source = backgroundDrawning};
        }

        private SnapshotSpan CreateSnapshotSpanForCurrentLine(ITextViewLine line)
        {
            Span lineSpan = Span.FromBounds(line.Start, line.End);
            return new SnapshotSpan(_view.TextSnapshot, lineSpan);
        }
    }
}


