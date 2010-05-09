using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.IntellisenseWindow;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    public class StepsIntellisenseProcessor : KeyProcessor
    {
        private readonly IWpfTextView _view;
        private bool justMovedCaret;
        private bool hasIntellisenseWindowOpen;
        private readonly IAdornmentLayer _layer;
        private readonly IntelliSenseControl _intellisenseWindow;
        readonly StepDefinitionFinder _finder;

        public StepsIntellisenseProcessor(IWpfTextView view)
        {
            _view = view;
            _layer = view.GetAdornmentLayer(AdornmentLayerNames.StepsIntellisense);
            _intellisenseWindow = new IntelliSenseControl();
            _view.Caret.PositionChanged += ResetCaretIfIntellisenseWindowOpen;

            _finder = new StepDefinitionFinder();
            _finder.NewStepsFound += OnNewStepsFound;
            _finder.ProcessSteps();
        }

        private void OnNewStepsFound(object sender, NewStepsFoundHandlerArgs args)
        {
            _intellisenseWindow.UpdatePopulatedView(args.NewDefinitions);
        }

        public static StepsIntellisenseProcessor Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new StepsIntellisenseProcessor(view));
        }
        
        public override void KeyUp(KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Down:
                    ChangeIntellisenseSelectedItem(HighlightedSelectionAction.Down);
                    break;
                case Key.Up:
                    ChangeIntellisenseSelectedItem(HighlightedSelectionAction.Up);
                    break;
                case Key.Left:
                case Key.Right:
                case Key.Escape:
                    CloseIntellisenseWindow();
                    break;
                case Key.Enter:
                    CompleteCurrentLineWithCurrentlySelectedLine();
                    break;
                default:
                    if(args.Key >= Key.A && args.Key <= Key.Z || args.Key == Key.Back || args.Key == Key.Delete)
                        UpdateIntellisense();
                    break;
            }

            args.Handled = true;
        }

        private void ShowIntellisenseWindow()
        {
            lock (_layer)
            {
                Canvas.SetLeft(_intellisenseWindow, 15);
                Canvas.SetTop(_intellisenseWindow, _view.Caret.Top + 15);
                _layer.Opacity = 100;
                _layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _intellisenseWindow, null);
                hasIntellisenseWindowOpen = true;
            }
        }

        private string GetTextForLine(ITextViewLine line)
        {
            SnapshotSpan snapshotSpan = GetSnapshotSpanForLine(line);            
            return snapshotSpan.GetText();
        }

        private SnapshotSpan GetSnapshotSpanForLine(ITextViewLine line)
        {
            Span lineSpan = Span.FromBounds(line.Start, line.End);
            return new SnapshotSpan(_view.TextSnapshot, lineSpan);
        }

        private ITextViewLine GetCurrentLine()
        {
            return _view.Caret.ContainingTextViewLine;
        }

        private void UpdateIntellisense()
        {
            if (!hasIntellisenseWindowOpen)
                ShowIntellisenseWindow();

            ITextViewLine line = GetCurrentLine();
            _intellisenseWindow.HighlightItem(GetTextForLine(line), GetCurrentCaratPosition());
        }

        private double GetCurrentCaratPosition()
        {
            return _view.Caret.ContainingTextViewLine.TextLeft;
        }

        private void CompleteCurrentLineWithCurrentlySelectedLine()
        {
            if(hasIntellisenseWindowOpen)
            {
                //string selectedText = _intellisenseWindow.GetCurrentlySelectedItem();
                //TODO: Move caret to previous line
                //Replace line with selected text
                CloseIntellisenseWindow();
            }
        }

        private void ChangeIntellisenseSelectedItem(HighlightedSelectionAction selectedAction)
        {
            _intellisenseWindow.ChangeSelection(selectedAction);
        }

        private void CloseIntellisenseWindow()
        {
            lock (_layer)
            {
                _layer.RemoveAdornment(_intellisenseWindow);
                hasIntellisenseWindowOpen = false;
            }
        }

        void ResetCaretIfIntellisenseWindowOpen(object sender, CaretPositionChangedEventArgs e)
        {
            CaretPosition oldPosition = e.OldPosition;

            if (!justMovedCaret && hasIntellisenseWindowOpen)
            {
                justMovedCaret = true;
                e.TextView.Caret.MoveTo(e.TextView.GetTextViewLineContainingBufferPosition(oldPosition.BufferPosition));
            }
            else
                justMovedCaret = false;
        }

    }

    public class StepDefinition
    {
        public string GWTType { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }

        public override string ToString()
        {
            return GWTType + " " + Name;
        }
    }

    internal class StepDefinitionFinder
    {
        internal event NewStepsFoundHandler NewStepsFound;

        public void ProcessSteps()
        {
            var stepDefinitions = new List<StepDefinition>
                                      {
                                          new StepDefinition
                                              {
                                                  GWTType = "Given",
                                                  File = "local.rb",
                                                  LineNumber = 123,
                                                  Name = "Something like...."
                                              },
                                          new StepDefinition
                                              {
                                                  GWTType = "When",
                                                  File = "local.rb",
                                                  LineNumber = 80,
                                                  Name = "Something like Or This"
                                              },
                                          new StepDefinition
                                              {
                                                  GWTType = "Then",
                                                  File = "local.rb",
                                                  LineNumber = 20,
                                                  Name = "But not this"
                                              }
                                      };
            RaiseNewStepsFound(stepDefinitions);
        }

        private void RaiseNewStepsFound(List<StepDefinition> stepDefinitions)
        {
            if (NewStepsFound == null)
                return;

            NewStepsFound(this, new NewStepsFoundHandlerArgs{NewDefinitions = stepDefinitions});
        }
    }

    internal delegate void NewStepsFoundHandler(object sender, NewStepsFoundHandlerArgs args);

    internal class NewStepsFoundHandlerArgs
    {
        public List<StepDefinition> NewDefinitions { get; set; }
    }
}


