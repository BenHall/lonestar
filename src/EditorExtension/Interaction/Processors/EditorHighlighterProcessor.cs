using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.OverlayDetails;
using Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    public sealed class EditorHighlighterProcessor : VsTextViewInteractions
    {
        private IEnumerable<FeatureResult> _featureResults;
        private readonly IAdornmentLayer _detailsLayer;
        protected override IWpfTextView View { set; get; }
        protected override IAdornmentLayer Layer { get; set; }

        public EditorHighlighterProcessor(IWpfTextView view)
        {
            View = view;
            Layer = view.GetAdornmentLayer(AdornmentLayerNames.EditorHighlighter);
            _detailsLayer = view.GetAdornmentLayer(AdornmentLayerNames.DetailsLayer);
            View.MouseHover += View_MouseHover;
        }

        void View_MouseHover(object sender, MouseHoverEventArgs e)
        {
            HideDetailsLayer();
            if(_featureResults == null)
                return;
            
            ITextSnapshotLine textSnapshotLine = e.View.TextSnapshot.GetLineFromPosition(e.Position);
            string step = textSnapshotLine.GetText();
            
            IEnumerable<StepResult> result = FindStepsWithName(step);

            foreach (var stepResult in result)
            {
                if (ShouldDisplayDetailsOverlay(stepResult))
                    DisplayDetailsOverlay(stepResult);
            }
        }

        private bool ShouldDisplayDetailsOverlay(StepResult stepResult)
        {
            return stepResult.Result == Result.Pending || stepResult.Result == Result.Failed;
        }

        private void HideDetailsLayer()
        {
            _detailsLayer.RemoveAdornmentsByTag(AdornmentLayerTags.DetailsOverlay);
        }

        private void DisplayDetailsOverlay(StepResult stepResult)
        {
            var details = new DetailsControl();
            details.SetStepDetails(stepResult);

            Canvas.SetLeft(details, 40);
            Canvas.SetTop(details, 40);

            _detailsLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, AdornmentLayerTags.DetailsOverlay, details, null);
        }

        private IEnumerable<StepResult> FindStepsWithName(string name)
        {
            return from featureResult in _featureResults
                   from scenarioResult in featureResult.ScenarioResults
                   from stepResult in scenarioResult.StepResults
                   where name.EndsWith(stepResult.Name)
                   select stepResult;
        }

        public void HighlightFeatureFileWithResults(IEnumerable<FeatureResult> featureResults)
        {
            _featureResults = featureResults;
            Layer.RemoveAdornmentsByTag(AdornmentLayerTags.ResultMarker);
            foreach (FeatureResult featureResult in featureResults)
            {
                foreach (ScenarioResult scenarioResult in featureResult.ScenarioResults)
                {
                    HighlightUIWithResults(scenarioResult);
                }
            }
        }

        private void HighlightUIWithResults(ScenarioResult scenarioResult)
        {
            foreach (IWpfTextViewLine uiLine in View.TextViewLines.WpfTextViewLines)
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

        private void HighlightLine(SnapshotSpan line, LineResultMarker marker)
        {
            IWpfTextViewLineCollection textViewLines = View.TextViewLines;
            Geometry geometry = textViewLines.GetMarkerGeometry(line);
            if (geometry != null)
            {
                Image highlight = CreateImageToHighlightLine(geometry, marker);

                Canvas.SetLeft(highlight, geometry.Bounds.Left);
                Canvas.SetTop(highlight, geometry.Bounds.Top);

                Layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, line, AdornmentLayerTags.ResultMarker, highlight, null);
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
    }
}