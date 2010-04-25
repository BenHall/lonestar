using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;
using Xunit;

namespace lonestar.tests.EditorExtensions.ResultAdapter
{
    public class ConvertOutputToObjectModelTests
    {
        string result = @"  feature_name
Feature: Search courses
  In order to ensure better utilization of courses
  Potential students should be able to search for courses
scenario_name
Search by topic
after_step_result
there are 240 courses which do not have the topic ""biology""
undefined
after_step_result
there are 2 courses A001, B205 that each have ""biology"" as one of the topics
undefined
after_step_result
I search for ""biology""
undefined
after_step_result
I should see the following courses:
undefined
steps_done
feature_done";

        [Fact]
        public void Convert_creates_features_objects_in_collection()
        {
            ConvertOutputToObjectModel model = new ConvertOutputToObjectModel();
            List<FeatureResult> featureResults = model.Convert(result);
            Assert.Equal(1, featureResults.Count);
        }

        [Fact]
        public void Convert_creates_scenarios_objects_in_collection()
        {
            ConvertOutputToObjectModel model = new ConvertOutputToObjectModel();
            List<FeatureResult> featureResults = model.Convert(result);
            Assert.Equal(1, featureResults[0].ScenarioResults.Count);
        }

        [Fact]
        public void Convert_creates_step_results_objects_in_collection()
        {
            ConvertOutputToObjectModel model = new ConvertOutputToObjectModel();
            List<FeatureResult> featureResults = model.Convert(result);
            Assert.Equal(4, featureResults[0].ScenarioResults[0].StepResults.Count);
        }


        [Fact]
        public void Step_result_has_result_attached()
        {
            ConvertOutputToObjectModel model = new ConvertOutputToObjectModel();
            List<FeatureResult> featureResults = model.Convert(result);
            Assert.Equal("undefined", featureResults[0].ScenarioResults[0].StepResults[0].ResultText);
        }

        [Fact]
        public void Step_result_has_name_of_step_attached()
        {
            ConvertOutputToObjectModel model = new ConvertOutputToObjectModel();
            List<FeatureResult> featureResults = model.Convert(result);
            Assert.Equal(@"there are 240 courses which do not have the topic ""biology""", featureResults[0].ScenarioResults[0].StepResults[0].Name);

        }
    }
}
