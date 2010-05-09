namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
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
}