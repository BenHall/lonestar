namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    public class StepDefinition
    {
        public string Name { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}