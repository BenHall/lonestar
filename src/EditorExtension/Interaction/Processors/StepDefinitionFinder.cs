using System.Collections.Generic;
using System.IO;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    internal delegate void NewStepsFoundHandler(object sender, NewStepsFoundHandlerArgs args);

    internal class StepDefinitionFinder
    {
        internal event NewStepsFoundHandler NewStepsFound;

        public void ProcessSteps(string getFilePath)
        {
            string directory = Path.GetDirectoryName(getFilePath);
            string[] rubyFilesToScan = Directory.GetFiles(directory, "*.rb", SearchOption.AllDirectories);
            var stepDefinitions = new List<StepDefinition>();

            foreach (var stepFile in rubyFilesToScan)
            {
                string[] readAllLines = File.ReadAllLines(stepFile);
                for (int index = 0; index < readAllLines.Length; index++)
                {
                    var readAllLine = readAllLines[index];
                    string line = readAllLine.Trim();
                    if (line.StartsWith("Given") || line.StartsWith("When") || line.StartsWith("Then"))
                        stepDefinitions.Add(new StepDefinition
                                                {
                                                    File = Path.GetFileName(stepFile),
                                                    LineNumber = index,
                                                    Name = line
                                                });
                }
            }

            RaiseNewStepsFound(stepDefinitions);
        }

        private void RaiseNewStepsFound(List<StepDefinition> stepDefinitions)
        {
            if (NewStepsFound == null)
                return;

            NewStepsFound(this, new NewStepsFoundHandlerArgs{NewDefinitions = stepDefinitions});
        }
    }
}