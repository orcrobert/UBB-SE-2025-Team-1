using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using WinUIApp.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class CategoryFilterComponent : UserControl
    {
        private IEnumerable<Category> Categories { get; set; }

        public event EventHandler<List<string>> CategoryChanged;

        public CategoryFilterComponent()
        {
            this.InitializeComponent();
        }

        public void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is Category selectedItem)
            {
                List<string> categoryFields = [];
                categoryFields.Add(selectedItem.Name);
                CategoryChanged?.Invoke(this, categoryFields);
            }
        }

        public void SetCategoriesFilter(IEnumerable<Category> categories)
        {
            Categories = categories;
            CategoryComboBox.ItemsSource = Categories;
        }

        public void ClearSelection()
        {
            CategoryComboBox.SelectedItem = null;
        }
    }
}
