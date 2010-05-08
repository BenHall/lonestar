using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.IntellisenseWindow
{
    /// <summary>
    /// Interaction logic for Selection.xaml
    /// </summary>
    public partial class Selection : UserControl
    {
        public Selection()
        {
            InitializeComponent();
        }

        public void HighlightItem(string text)
        {
            label1.Content = text;
        }
    }
}
