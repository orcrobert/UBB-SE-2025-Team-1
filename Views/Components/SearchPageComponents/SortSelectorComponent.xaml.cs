using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class SortSelectorComponent : UserControl
    {
        public SortSelectorComponent()
        {
            this.InitializeComponent();
        }
        public event EventHandler<bool> SortOrderChanged;


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
