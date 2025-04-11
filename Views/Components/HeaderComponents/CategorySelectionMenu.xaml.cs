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
        private List<Category> originalCategories = new List<Category>();
        private HashSet<Category> selectedCategories = new HashSet<Category>();
        public ObservableCollection<Category> CurrentCategories { get; set; } = new ObservableCollection<Category>();
        public HashSet<Category> SelectedCategories => selectedCategories;

        /// <summary>
        /// Initializes a new instance of the CategorySelectionMenu control.
        /// </summary>
        public CategorySelectionMenu()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the menu with the provided list of categories and stores them for filtering operations.
        /// </summary>
        /// <param name="categories">The list of categories to populate the menu with.</param>
        public void PopulateCategories(List<Category> categories)
        {
            originalCategories = categories;
            CurrentCategories = new ObservableCollection<Category>(categories);
        }

        /// <summary>
        /// Handles selection changes in the category list by updating the selectedCategories collection.
        /// Adds newly selected categories and removes deselected ones.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="selectionChangedEventArgs">Event data containing removed and added items.</param>
        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            foreach (Category removedCategory in selectionChangedEventArgs.RemovedItems)
            {
                selectedCategories.Remove(removedCategory);
            }
            foreach (Category addedCategory in selectionChangedEventArgs.AddedItems)
            {
                selectedCategories.Add(addedCategory);
            }
        }

        /// <summary>
        /// Filters the category list based on user input in the search box.
        /// Temporarily detaches the selection event handler to preserve selected items during filtering.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="textChangedEventArgs">Event data for the text changed event.</param>
        private void CategorySearchBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            string searchQuery = CategorySearchBox.Text.ToLower();
            List<Category> filteredCategories = originalCategories
                .Where(category => category.CategoryName.ToLower().Contains(searchQuery))
                .ToList();
            CategoryList.SelectionChanged -= CategoryList_SelectionChanged;
            CurrentCategories.Clear();
            foreach (Category category in filteredCategories)
            {
                CurrentCategories.Add(category);
            }
            CategoryList.SelectedItems.Clear();
            foreach (Category category in filteredCategories)
            {
                if (selectedCategories.Contains(category))
                {
                    CategoryList.SelectedItems.Add(category);
                }
            }
            CategoryList.SelectionChanged += CategoryList_SelectionChanged;
        }
    }
}
