using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WinUIApp.Models;

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class CategoryFilterComponent : UserControl
    {
        private List<Category> _originalCategories = new List<Category>();
        private HashSet<Category> _selectedCategories = new HashSet<Category>();
        public ObservableCollection<Category> CurrentCategories { get; set; } = new ObservableCollection<Category>();

        public event EventHandler<List<string>> CategoryChanged;

        public CategoryFilterComponent()
        {
            this.InitializeComponent();
            Debug.WriteLine("selected in component" + CategoryList.SelectedItems.Count);
        }

        public void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Category removedCategory in e.RemovedItems)
                _selectedCategories.Remove(removedCategory);

            foreach (Category addedCategory in e.AddedItems)
                _selectedCategories.Add(addedCategory);
            CategoryChanged?.Invoke(this, _selectedCategories.Select(c => c.Name).ToList());
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();

            List<Category> filteredCategories = _originalCategories
                .Where(category => category.Name.ToLower().Contains(query))
                .ToList();

            CategoryList.SelectionChanged -= CategoryListView_SelectionChanged;

            CurrentCategories.Clear();
            foreach (Category category in filteredCategories)
            {
                CurrentCategories.Add(category);
            }

            CategoryList.SelectedItems.Clear();
            foreach (Category category in filteredCategories)
            {
                if (_selectedCategories.Contains(category))
                {
                    CategoryList.SelectedItems.Add(category);
                }
            }

            CategoryList.SelectionChanged += CategoryListView_SelectionChanged;
        }

        public async void SetCategoriesFilter(IEnumerable<Category> categories, IEnumerable<Category> initialCategories)
        {
            _originalCategories = categories.ToList();
            CurrentCategories.Clear();
            foreach (Category category in _originalCategories)
            {
                CurrentCategories.Add(category);
            }

            HashSet<int> ids = new HashSet<int>();
            foreach (Category category in initialCategories)
            {
                ids.Add(category.Id);
            }
            CategoryList.SelectedItems.Clear();

            await Task.Delay(50);
            foreach (Category category in _originalCategories)
            {
                if (ids.Contains(category.Id))
                {
                    CategoryList.SelectedItems.Add(category);
                    _selectedCategories.Add(category);
                }
            }
        }

        public void ClearSelection()
        {
            CategoryList.SelectedItems.Clear();
            _selectedCategories.Clear();
            CategoryChanged?.Invoke(this, new List<string>());
        }
    }
}