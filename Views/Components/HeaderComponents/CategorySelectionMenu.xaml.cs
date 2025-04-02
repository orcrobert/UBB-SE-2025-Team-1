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

        private List<Category> _originalCategories;
        public ObservableCollection<Category> CurrentCategories { get; set; }

        public CategorySelectionMenu()
        {
            this.InitializeComponent();
            PopulateCategories(new List<Category> {new Category(1, "abc"), new Category(1, "def"), new Category(1, "abc"), new Category(1, "ABc") });
        }

        public void PopulateCategories(List<Category> categories)
        {
            _originalCategories = categories;
            CurrentCategories = new ObservableCollection<Category>(categories);
        }

        private void CategorySearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = CategorySearchBox.Text.ToLower();
            List<Category> filteredCategories = _originalCategories.Where(category => category.Name.ToLower().Contains(query)).ToList();
            CurrentCategories.Clear();
            foreach (Category category in filteredCategories)
                CurrentCategories.Add(category);
        }
    }
}
