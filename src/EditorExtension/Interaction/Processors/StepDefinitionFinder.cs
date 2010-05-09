using System.Collections.Generic;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    internal delegate void NewStepsFoundHandler(object sender, NewStepsFoundHandlerArgs args);

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
}