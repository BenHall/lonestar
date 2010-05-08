﻿using Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors;

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

        public void ChangeSelection(HighlightedSelection selectedAction)
        {
            int selectedIndex = listBox1.SelectedIndex;

            switch (selectedAction)
            {
                case HighlightedSelection.Up:
                    if (selectedIndex == 0)
                        listBox1.SelectedIndex = (listBox1.Items.Count - 1);

                    listBox1.SelectedIndex--;
                    break;

                case HighlightedSelection.Down:
                    if (selectedIndex == (listBox1.Items.Count - 1))
                        listBox1.SelectedIndex = 0;

                    listBox1.SelectedIndex++;
                    break;
            }
        }
    }
}
