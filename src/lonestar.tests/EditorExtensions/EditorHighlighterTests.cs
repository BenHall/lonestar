﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media.TextFormatting;
using Meerkatalyst.Lonestar.EditorExtension;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Rhino.Mocks;
using Xunit;
using Rhino.Mocks.Constraints;
using TextBounds = Microsoft.VisualStudio.Text.Formatting.TextBounds;

namespace lonestar.tests.EditorExtensions
{
    public class EditorHighlighterTests
    {
        [Fact]
        public void should_remove_existing_layers_from_view()
        {
            IAdornmentLayer stubLayer = MockRepository.GenerateStub<IAdornmentLayer>();
            IWpfTextView stubView = MockRepository.GenerateStub<IWpfTextView>();
            stubView.Stub(v => v.GetAdornmentLayer("EditorHighlighter")).Return(stubLayer);

            EditorHighlighter editor = new EditorHighlighter(stubView);
            editor.HighlightFeatureFileWithResults(new List<FeatureResult>());

            stubLayer.AssertWasCalled(l => l.RemoveAllAdornments());
            stubLayer.AssertWasCalled(l => l.RemoveAdornmentsByTag("ResultMarker"));
        }

        //[Fact]
        //public void should_add_adornment_for_each_result()
        //{
        //    IAdornmentLayer mockLayer = MockRepository.GenerateMock<IAdornmentLayer>();
        //    IWpfTextView stubView = MockRepository.GenerateStub<IWpfTextView>();
        //    IWpfTextView stubTextLine = MockRepository.GenerateStub<IWpfTextView>();
        //    stubView.Stub(v => v.GetAdornmentLayer("EditorHighlighter")).Return(mockLayer);
        //    stubTextLine.Stub(l => l.Start).Return(new SnapshotPoint());
        //    stubTextLine.Stub(l => l.End).Return(new SnapshotPoint());
        //    stubView.Stub(v => v.TextViewLines).Return(new List<IWpfTextView> {stubTextLine});

        //    mockLayer.Expect(l => l.AddAdornment(AdornmentPositioningBehavior.TextRelative, null, null, null, null)).IgnoreArguments().Constraints(Is.Equal(AdornmentPositioningBehavior.TextRelative), Is.TypeOf(typeof(SnapshotSpan)), Is.Equal("ResultMarker"), Is.TypeOf(typeof (Image)), Is.Null());
        //    EditorHighlighter editor = new EditorHighlighter(stubView);
        //    editor.HighlightFeatureFileWithResults(CreateResults());
        //    mockLayer.VerifyAllExpectations();
        //}

        private List<FeatureResult> CreateResults()
        {
            var features = new List<FeatureResult>();

            FeatureResult result = new FeatureResult();

            ScenarioResult scenarioResult = new ScenarioResult();

            StepResult stepResult = new StepResult();
            stepResult.Name = "Test";
            stepResult.Result = "passed";

            scenarioResult.StepResults.Add(stepResult);

            result.ScenarioResults.Add(scenarioResult);

            features.Add(result);

            return features;
        }
    }
}