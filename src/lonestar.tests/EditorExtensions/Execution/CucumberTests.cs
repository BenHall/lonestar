using System;
using Meerkatalyst.Lonestar.EditorExtension.Execution;
using Xunit;

namespace lonestar.tests.EditorExtensions.Execution
{
    public class CucumberTests
    {
        [Fact]
        public void Returns_results_from_execution()
        {
            string path = @"D:\SourceControl\lonestar\example\hello_world.feature";

            Cucumber cucumber = new Cucumber(path);
            string execute = cucumber.Execute();
            Console.WriteLine(execute);
            Assert.True(!string.IsNullOrEmpty(execute), execute);
        }

        [Fact]
        public void GetArguments()
        {
            string path = @"D:\SourceControl\lonestar\example\hello_world.feature";

            Cucumber cucumber = new Cucumber(path);
            string result = cucumber.GetArguments();
            Console.WriteLine(result);
            Assert.True(!string.IsNullOrEmpty(result), result);
        }
    }
}


