using System;
using System.Collections.Generic;
using Meerkatalyst.Lonestar.EditorExtension.Interaction.Processors;

namespace Meerkatalyst.Lonestar.EditorExtension.Interaction.IntellisenseWindow
{
    public partial class IntelliSenseControl
    {
        public IntelliSenseControl()
        {
            InitializeComponent();
        }

        public void HighlightItem(string text, double getCurrentCaratPosition)
        {
            string substringOfText = text.Substring(0, Int32.Parse(getCurrentCaratPosition.ToString()));

            foreach (StepDefinition item in listBox1.Items)
            {
                if (item.ToString().StartsWith(text.Trim()) || item.ToString().StartsWith(substringOfText))
                {
                    listBox1.SelectedItem = item;
                    listBox1.ScrollIntoView(item);
                    return;
                }
            }


            listBox1.SelectedIndex = -1;
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
            var currentlySelectedItem = listBox1.SelectedItem as StepDefinition;
            return currentlySelectedItem == null ? string.Empty : currentlySelectedItem.ToString();
        }

        public void UpdatePopulatedView(List<StepDefinition> intellisenseWindow)
        {
            foreach (var stepDefinition in intellisenseWindow)
                listBox1.Items.Add(stepDefinition);
        }
    }
}
