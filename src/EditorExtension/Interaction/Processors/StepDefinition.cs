namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors
{
    public class StepDefinition
    {
        public string Name { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }

        public override string ToString()
        {
            return VisbleString;
        }

        public string CleanedName { get { return CleanName(); } }
        public string FileDetails { get { return File + ":" + LineNumber; } }
        public string VisbleString { get { return CleanedName + "\t\t" + FileDetails; } }

        private string CleanName()
        {
            string nameWithoutDo = Name.Substring(0, Name.LastIndexOf("do"));
            string nameWithoutRegEx = nameWithoutDo.Replace("/^", "").Replace("$/", "");
            return nameWithoutRegEx;
        }
    }
}