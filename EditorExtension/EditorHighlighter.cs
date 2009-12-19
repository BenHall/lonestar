using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter;
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
            //_view.LayoutChanged += OnLayoutChanged;
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                CreateVisuals(line, new Fail());
            }
        }

        private void CreateVisuals(ITextViewLine line, LineResultMarker resultMarker)
        {
            int start = line.Start;
            int end = line.End;

            SnapshotSpan span = new SnapshotSpan(_view.TextSnapshot, Span.FromBounds(start, end));
            HighlightLine(span, resultMarker);
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

        public static EditorHighlighter Instance { get; set; }
        //TODO: Use MEF!
        public static void Create(IWpfTextView textView)
        {
            if (Instance == null)
                Instance = new EditorHighlighter(textView);
        }

        public void UpdateUI(List<FeatureResult> featureResults)
        {
            foreach (FeatureResult featureResult in featureResults)
            {
                foreach (ScenarioResult scenarioResult in featureResult.ScenarioResults)
                {
                    HighlightStepsWithResults(scenarioResult);
                }
            }

        }

        private void HighlightStepsWithResults(ScenarioResult scenarioResult)
        {
            foreach (StepResult stepResult in scenarioResult.StepResults)
            {
                foreach (ITextViewLine line in _view.TextViewLines)
                {
                    if(line.Snapshot.GetText().Equals(stepResult.Name))
                    {
                        LineResultMarker resultMarker = GetResultMarker(stepResult.Result);
                        CreateVisuals(line, resultMarker);
                    }
                }
            }
        }

        private LineResultMarker GetResultMarker(string result)
        {
            switch (result)
            {
                case "passed":
                    return new Pass();
                case "failed":
                    return new Fail();
            }

            return new Pending();
        }
    }
}


