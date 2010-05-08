using System;
using System.Windows.Controls;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.IntellisenseWindow
{
    public partial class IntelliSenseControl
    {
        public IntelliSenseControl()
        {
            InitializeComponent();
        }

        public void HighlightItem(string text)
        {
            listBox1.Items.Add(text);
        }

        public void ChangeSelection(HighlightedSelectionAction selectedAction)
        {
            int selectedIndex = listBox1.SelectedIndex;

            switch (selectedAction)
            {
                case HighlightedSelectionAction.Up:
                    if (selectedIndex == 0)
                    {
                        listBox1.SelectedIndex = (listBox1.Items.Count - 1);
                        listBox1.ScrollIntoView(listBox1.SelectedIndex);
                    }

                    listBox1.SelectedIndex--;
                    break;

                case HighlightedSelectionAction.Down:
                    if (selectedIndex == (listBox1.Items.Count - 1))
                    {
                        listBox1.SelectedIndex = 0;
                        listBox1.ScrollIntoView(listBox1.SelectedIndex);
                    }

                    listBox1.SelectedIndex++;
                    break;
            }
        }

        public string GetCurrentlySelectedItem()
        {
            return listBox1.SelectedItem as string;
        }
    }
}
