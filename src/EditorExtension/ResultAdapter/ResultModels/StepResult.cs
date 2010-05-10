namespace Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels
{
    public class StepResult
    {
        public string Name { get; set; }
        public string ResultText { get; set; }

        //TODO: These should be sub-classes
        public string Exception { get; set; }
        public string StackTrace { get; set; }

        public Result Result
        {
            get
            {
                switch (ResultText)
                {
                    case "passed":
                        return Result.Passed;
                    case "failed":
                        return Result.Failed;
                    case "skipped":
                        return Result.Skipped;
                    case "pending":
                        return Result.Pending;
                }

                return Result.Unknown;
            }
        }
    }

    public enum Result
    {
        Passed,
        Failed,
        Pending,
        Skipped,
        Unknown
    }
}