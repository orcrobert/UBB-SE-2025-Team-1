using Microsoft.UI.Xaml.Controls;
using System;


namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class SortSelectorComponent : UserControl
    {

        public event EventHandler<bool> SortOrderChanged;
        public event EventHandler<string> SortByChanged;

        public SortSelectorComponent()
        {
            this.InitializeComponent();
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortByComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortField = selectedItem.Content.ToString();
                SortByChanged?.Invoke(this, sortField);
            }
        }

        public void SetSortBy(string sortField)
        {
            switch (sortField)
            {
                case "Name":
                    SortByComboBox.SelectedIndex = 0;
                    break;
                case "Alcohol Content":
                    SortByComboBox.SelectedIndex = 1;
                    break;
                case "Average Review Score":
                    SortByComboBox.SelectedIndex = 2;
                    break;
            }
        }

        private void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortOrderComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                bool isAscending = selectedItem.Content.ToString() == "Ascending";
                SortOrderChanged?.Invoke(this, isAscending);
            }
        }

        public void SetSortOrder(bool isAscending)
        {
            SortOrderComboBox.SelectedIndex = isAscending ? 0 : 1;
        }
    }
}
