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
        private List<Category> originalCategories = new List<Category>();
        private HashSet<Category> selectedCategories = new HashSet<Category>();
        private const int SelectionDelayMilliseconds = 50;
        public ObservableCollection<Category> CurrentCategories { get; set; } = new ObservableCollection<Category>();

        /// <summary>
        /// Event that fires when the selected categories change, providing a list of selected category names.
        /// </summary>
        public event EventHandler<List<string>> CategoryChanged;

        /// <summary>
        /// Initializes a new instance of the CategoryFilterComponent control and outputs debug information.
        /// </summary>
        public CategoryFilterComponent()
        {
            this.InitializeComponent();
            Debug.WriteLine("selected in component" + CategoryList.SelectedItems.Count);
        }

        /// <summary>
        /// Handles selection changes in the category list by updating the selectedCategories collection
        /// and triggering the CategoryChanged event with the updated list of category names.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="selectionChangedEventArgs">Event data containing removed and added items.</param>
        public void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            foreach (Category removedCategory in selectionChangedEventArgs.RemovedItems)
            {
                selectedCategories.Remove(removedCategory);
            }
            foreach (Category addedCategory in selectionChangedEventArgs.AddedItems)
            {
                selectedCategories.Add(addedCategory);
            }
            CategoryChanged?.Invoke(this, selectedCategories.Select(category => category.Name).ToList());
        }

        /// <summary>
        /// Filters the category list based on user input in the search box while preserving selections.
        /// Temporarily detaches the selection event handler during the update process.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="textChangedEventArgs">Event data for the text changed event.</param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            string searchQuery = SearchBox.Text.ToLower();
            List<Category> filteredCategories = originalCategories
                .Where(category => category.Name.ToLower().Contains(searchQuery))
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
                if (selectedCategories.Contains(category))
                {
                    CategoryList.SelectedItems.Add(category);
                }
            }

            CategoryList.SelectionChanged += CategoryListView_SelectionChanged;
        }

        /// <summary>
        /// Updates the available categories for filtering and sets initial selections based on provided categories.
        /// Uses a short delay to ensure UI can update properly before setting selections.
        /// </summary>
        /// <param name="categories">The collection of categories to be used for filtering.</param>
        /// <param name="initialCategories">The collection of categories that should be initially selected.</param>
        public async void SetCategoriesFilter(IEnumerable<Category> categories, IEnumerable<Category> initialCategories)
        {
            originalCategories = categories.ToList();
            CurrentCategories.Clear();
            foreach (Category category in originalCategories)
            {
                CurrentCategories.Add(category);
            }
            HashSet<int> categoryIdentifiers = new HashSet<int>();
            if (initialCategories != null)
            {
                foreach (Category category in initialCategories)
                {
                    categoryIdentifiers.Add(category.Id);
                }
            }
            CategoryList.SelectedItems.Clear();
            await Task.Delay(SelectionDelayMilliseconds);
            foreach (Category category in originalCategories)
            {
                if (categoryIdentifiers.Contains(category.Id))
                {
                    CategoryList.SelectedItems.Add(category);
                    selectedCategories.Add(category);
                }
            }
        }

        /// <summary>
        /// Clears all selected categories and triggers the CategoryChanged event with an empty list.
        /// </summary>
        public void ClearSelection()
        {
            CategoryList.SelectedItems.Clear();
            selectedCategories.Clear();
            CategoryChanged?.Invoke(this, new List<string>());
        }
    }
}