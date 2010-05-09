using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    internal delegate void NewStepsFoundHandler(object sender, NewStepsFoundHandlerArgs args);

    internal class StepDefinitionFinder
    {
        internal event NewStepsFoundHandler NewStepsFound;

        public void ProcessSteps(string getFilePath)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s,e) => { e.Result = CreateStepsForAllFilesInPath(e.Argument.ToString()); };

            worker.RunWorkerCompleted += (s, e) => RaiseNewStepsFound(e.Result as List<StepDefinition>);
            worker.RunWorkerAsync(getFilePath);
        }

        private List<StepDefinition> CreateStepsForAllFilesInPath(string getFilePath)
        {
            string directory = Path.GetDirectoryName(getFilePath);
            string[] rubyFilesToScan = Directory.GetFiles(directory, "*.rb",
                                                          SearchOption.AllDirectories);
            var stepDefinitions = new List<StepDefinition>();

            foreach (var stepFile in rubyFilesToScan)
            {
                stepDefinitions.AddRange(GetStepsForFile(stepFile));
            }

            return stepDefinitions;
        }

        private IEnumerable<StepDefinition> GetStepsForFile(string stepFile)
        {
            List<StepDefinition> stepDefinitions = new List<StepDefinition>();
            string[] readAllLines = File.ReadAllLines(stepFile);
            for (int index = 0; index < readAllLines.Length; index++)
            {
                var readAllLine = readAllLines[index];
                string line = readAllLine.Trim();
                if (StepDefinition(line))
                    stepDefinitions.Add(new StepDefinition
                                            {
                                                File = Path.GetFileName(stepFile),
                                                LineNumber = index,
                                                Name = line
                                            });
            }

            return stepDefinitions;
        }

        private bool StepDefinition(string line)
        {
            return line.StartsWith("Given") || line.StartsWith("When") || line.StartsWith("Then");
        }

        private void RaiseNewStepsFound(List<StepDefinition> stepDefinitions)
        {
            if (NewStepsFound == null || stepDefinitions == null || stepDefinitions.Count == 0)
                return;

            NewStepsFound(this, new NewStepsFoundHandlerArgs{NewDefinitions = stepDefinitions});
        }
    }
}