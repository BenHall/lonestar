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
            listBox1.SelectedItem = text;
            listBox1.ScrollIntoView(text);
        }

        public void ChangeSelection(HighlightedSelectionAction selectedAction)
        {
            int selectedIndex = listBox1.SelectedIndex;

            switch (selectedAction)
            {
                case HighlightedSelectionAction.Up:
                    if (selectedIndex == 0)
                        listBox1.SelectedIndex = (listBox1.Items.Count - 1);
                    else
                        listBox1.SelectedIndex = selectedIndex - 1;

                    listBox1.ScrollIntoView(listBox1.SelectedItem);
                    break;

                case HighlightedSelectionAction.Down:
                    if (selectedIndex == (listBox1.Items.Count - 1))
                        listBox1.SelectedIndex = 0;
                    else
                        listBox1.SelectedIndex = selectedIndex + 1;

                    listBox1.ScrollIntoView(listBox1.SelectedItem);
                    break;
            }
        }

        public string GetCurrentlySelectedItem()
        {
            return listBox1.SelectedItem as string;
        }
    }
}
