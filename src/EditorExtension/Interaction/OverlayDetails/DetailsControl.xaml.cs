using System.Windows.Controls;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.OverlayDetails
{
    /// <summary>
    /// Interaction logic for DetailsControl.xaml
    /// </summary>
    public partial class DetailsControl
    {
        public DetailsControl()
        {
            InitializeComponent();
        }

        public void SetStepDetails(StepResult stepResult)
        {
            label1.Content = stepResult.Name + "\r\n" + stepResult.Exception;
        }
    }
}
