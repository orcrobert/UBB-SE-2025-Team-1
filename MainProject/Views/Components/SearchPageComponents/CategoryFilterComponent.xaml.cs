// <copyright file="CategoryFilterComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.Models;

    /// <summary>
    /// A UserControl that provides a category filter for a list of items.
    /// </summary>
    public sealed partial class CategoryFilterComponent : UserControl
    {
        private const int SelectionDelayMilliseconds = 50;
        private List<Category> originalCategories = new List<Category>();
        private HashSet<Category> selectedCategories = new HashSet<Category>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryFilterComponent"/> class.
        /// </summary>
        public CategoryFilterComponent()
        {
            this.InitializeComponent();
            Debug.WriteLine("selected in component" + this.CategoryList.SelectedItems.Count);
        }

        /// <summary>
        /// Event that fires when the selected categories change, providing a list of selected category names.
        /// </summary>
        public event EventHandler<List<string>> CategoryChanged;

        /// <summary>
        /// Gets or sets the list of all available categories for filtering.
        /// </summary>
        public ObservableCollection<Category> CurrentCategories { get; set; } = new ObservableCollection<Category>();

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
                this.selectedCategories.Remove(removedCategory);
            }

            foreach (Category addedCategory in selectionChangedEventArgs.AddedItems)
            {
                this.selectedCategories.Add(addedCategory);
            }

            this.CategoryChanged?.Invoke(this, this.selectedCategories.Select(category => category.CategoryName).ToList());
        }

        /// <summary>
        /// Updates the available categories for filtering and sets initial selections based on provided categories.
        /// Uses a short delay to ensure UI can update properly before setting selections.
        /// </summary>
        /// <param name="categories">The collection of categories to be used for filtering.</param>
        /// <param name="initialCategories">The collection of categories that should be initially selected.</param>
        public async void SetCategoriesFilter(IEnumerable<Category> categories, IEnumerable<Category> initialCategories)
        {
            this.originalCategories = categories.ToList();
            this.CurrentCategories.Clear();
            foreach (Category category in this.originalCategories)
            {
                this.CurrentCategories.Add(category);
            }

            HashSet<int> categoryIdentifiers = new HashSet<int>();
            if (initialCategories != null)
            {
                foreach (Category category in initialCategories)
                {
                    categoryIdentifiers.Add(category.CategoryId);
                }
            }

            this.CategoryList.SelectedItems.Clear();
            await Task.Delay(SelectionDelayMilliseconds);
            foreach (Category category in this.originalCategories)
            {
                if (categoryIdentifiers.Contains(category.CategoryId))
                {
                    this.CategoryList.SelectedItems.Add(category);
                    this.selectedCategories.Add(category);
                }
            }
        }

        /// <summary>
        /// Clears all selected categories and triggers the CategoryChanged event with an empty list.
        /// </summary>
        public void ClearSelection()
        {
            this.CategoryList.SelectedItems.Clear();
            this.selectedCategories.Clear();
            this.CategoryChanged?.Invoke(this, new List<string>());
        }

        /// <summary>
        /// Filters the category list based on user input in the search box while preserving selections.
        /// Temporarily detaches the selection event handler during the update process.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="textChangedEventArgs">Event data for the text changed event.</param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            string searchQuery = this.SearchBox.Text.ToLower();
            List<Category> filteredCategories = this.originalCategories
                .Where(category => category.CategoryName.ToLower().Contains(searchQuery))
                .ToList();

            this.CategoryList.SelectionChanged -= this.CategoryListView_SelectionChanged;

            this.CurrentCategories.Clear();
            foreach (Category category in filteredCategories)
            {
                this.CurrentCategories.Add(category);
            }

            this.CategoryList.SelectedItems.Clear();
            foreach (Category category in filteredCategories)
            {
                if (this.selectedCategories.Contains(category))
                {
                    this.CategoryList.SelectedItems.Add(category);
                }
            }

            this.CategoryList.SelectionChanged += this.CategoryListView_SelectionChanged;
        }
    }
}