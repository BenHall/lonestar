namespace Meerkatalyst.Lonestar.EditorExtension.Interaction
{
    public abstract class StatusUpdater
    {
        public delegate void UpdatedStatusEventHandler(object sender, StatusEventArgs e);
        public event UpdatedStatusEventHandler UpdatedStatus;

        protected virtual void OnUpdatedStatus(StatusEventArgs e)
        {
            if (UpdatedStatus != null)
                UpdatedStatus(this, e);
        }
    }

    public class StatusEventArgs
    {
        /// <summary>
        /// Single line message about what happened
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// More in-depth information about what happened
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Clear the output
        /// </summary>
        public bool Clear { get; set; }
    }
}