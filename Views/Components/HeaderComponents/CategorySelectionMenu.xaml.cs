using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using WinUIApp.Models;
using System.Collections.ObjectModel;

namespace WinUIApp.Views.Components.HeaderComponents
{
    public sealed partial class CategorySelectionMenu : UserControl
    {

        private List<Category> _originalCategories = new List<Category>();
        private HashSet<Category> _selectedCategories = new HashSet<Category>();
        public ObservableCollection<Category> CurrentCategories { get; set; } = new ObservableCollection<Category>();
        public HashSet<Category> SelectedCategories => _selectedCategories;

        public CategorySelectionMenu()
        {
            this.InitializeComponent();
        }

        public void PopulateCategories(List<Category> categories)
        {
            _originalCategories = categories;
            CurrentCategories = new ObservableCollection<Category>(categories);
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Category removedCategory in e.RemovedItems)
                _selectedCategories.Remove(removedCategory);
            foreach (Category addedCategory in e.AddedItems)
                _selectedCategories.Add(addedCategory);
        }

        private void CategorySearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = CategorySearchBox.Text.ToLower();
            List<Category> filteredCategories = _originalCategories.Where(category => category.Name.ToLower().Contains(query)).ToList();
            CategoryList.SelectionChanged -= CategoryList_SelectionChanged;
            CurrentCategories.Clear();
            foreach (Category category in filteredCategories)
                CurrentCategories.Add(category);
            CategoryList.SelectedItems.Clear();
            foreach (Category category in filteredCategories)
            {
                if (_selectedCategories.Contains(category))
                    CategoryList.SelectedItems.Add(category);
            }
            CategoryList.SelectionChanged += CategoryList_SelectionChanged;
        }
    }
}
