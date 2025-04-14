// <copyright file="CategorySelectionMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components.HeaderComponents
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Navigation;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using WinUIApp.Models;

    /// <summary>
    /// Represents a user control for selecting categories in a menu.
    /// </summary>
    public sealed partial class CategorySelectionMenu : UserControl
    {
        private List<Category> originalCategories = new List<Category>();
        private HashSet<Category> selectedCategories = new HashSet<Category>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySelectionMenu"/> class.
        /// </summary>
        public CategorySelectionMenu()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the current list of categories displayed in the menu.
        /// </summary>
        public ObservableCollection<Category> CurrentCategories { get; set; } = new ObservableCollection<Category>();

        /// <summary>
        /// Gets the selected categories from the menu.
        /// </summary>
        public HashSet<Category> SelectedCategories => this.selectedCategories;

        /// <summary>
        /// Populates the menu with the provided list of categories and stores them for filtering operations.
        /// </summary>
        /// <param name="categories">The list of categories to populate the menu with.</param>
        public void PopulateCategories(List<Category> categories)
        {
            this.originalCategories = categories;
            this.CurrentCategories = new ObservableCollection<Category>(categories);
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
                this.selectedCategories.Remove(removedCategory);
            }

            foreach (Category addedCategory in selectionChangedEventArgs.AddedItems)
            {
                this.selectedCategories.Add(addedCategory);
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
            string searchQuery = this.CategorySearchBox.Text.ToLower();
            List<Category> filteredCategories = this.originalCategories
                .Where(category => category.CategoryName.ToLower().Contains(searchQuery))
                .ToList();
            this.CategoryList.SelectionChanged -= this.CategoryList_SelectionChanged;
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

            this.CategoryList.SelectionChanged += this.CategoryList_SelectionChanged;
        }
    }
}