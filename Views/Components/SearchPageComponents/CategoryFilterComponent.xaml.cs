using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class CategoryFilterComponent : UserControl
    {
        private IEnumerable<Category> Categories { get; set; }
        private IEnumerable<Category> FilteredCategories { get; set; }

        private List<string> _categorySelectedFields = [];

        public event EventHandler<List<string>> CategoryChanged;

        public CategoryFilterComponent()
        {
            this.InitializeComponent();
        }

        public void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.Cast<Category>())
                _categorySelectedFields.Remove(removed.Name);

            foreach (var added in e.AddedItems.Cast<Category>())
                _categorySelectedFields.Add(added.Name);


            CategoryChanged?.Invoke(this, _categorySelectedFields);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            var selectedItems = CategoryList.SelectedItems.Cast<Category>().ToList();

            FilteredCategories = Categories
                .Where(c => c.Name.ToLower().Contains(query)).Cast<Category>();
            CategoryList.ItemsSource = FilteredCategories;

            foreach (var item in selectedItems)
            {
                if (FilteredCategories.Contains(item))
                    CategoryList.SelectedItems.Add(item);
            }
        }


        public void SetCategoriesFilter(IEnumerable<Category> categories)
        {
            Categories = categories;
            FilteredCategories = Categories;
            CategoryList.ItemsSource = Categories;
        }

        public void ClearSelection()
        {
            CategoryList.SelectedItem = null;
        }
    }
}
