// <copyright file="BrandFilterComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.Models;

    /// <summary>
    /// A user control for filtering drinks by brand.
    /// </summary>
    public sealed partial class BrandFilterComponent : UserControl
    {
        private List<Brand> originalBrands = new List<Brand>();
        private HashSet<Brand> selectedBrands = new HashSet<Brand>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandFilterComponent"/> class.
        /// </summary>
        public BrandFilterComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event that fires when the selected brands change, providing a list of selected brand names.
        /// </summary>
        public event EventHandler<List<string>> BrandChanged;

        /// <summary>
        /// Gets or sets the list of currently available brands for filtering.
        /// </summary>
        public ObservableCollection<Brand> CurrentBrands { get; set; } = new ObservableCollection<Brand>();

        /// <summary>
        /// Updates the available brands for filtering and restores any previous selections
        /// that are still valid with the new brand list.
        /// </summary>
        /// <param name="brands">The collection of brands to be used for filtering.</param>
        public void SetBrandFilter(IEnumerable<Brand> brands)
        {
            this.originalBrands = brands?.ToList() ?? new List<Brand>();
            this.CurrentBrands.Clear();
            foreach (Brand brand in this.originalBrands)
            {
                this.CurrentBrands.Add(brand);
            }

            this.BrandList.SelectedItems.Clear();
            foreach (Brand brand in this.CurrentBrands)
            {
                if (this.selectedBrands.Contains(brand))
                {
                    this.BrandList.SelectedItems.Add(brand);
                }
            }
        }

        /// <summary>
        /// Clears all selected brands and triggers the BrandChanged event with an empty list.
        /// </summary>
        public void ClearSelection()
        {
            this.BrandList.SelectedItems.Clear();
            this.selectedBrands.Clear();
            this.BrandChanged?.Invoke(this, new List<string>());
        }

        /// <summary>
        /// Handles selection changes in the brand list by updating the selectedBrands collection
        /// and triggering the BrandChanged event with the updated list of brand names.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="selectionChangedEventArgs">Event data containing removed and added items.</param>
        private void BrandListView_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            foreach (Brand removedBrand in selectionChangedEventArgs.RemovedItems)
            {
                this.selectedBrands.Remove(removedBrand);
            }

            foreach (Brand addedBrand in selectionChangedEventArgs.AddedItems)
            {
                this.selectedBrands.Add(addedBrand);
            }

            this.BrandChanged?.Invoke(this, this.selectedBrands.Select(brand => brand.BrandName).ToList());
        }

        /// <summary>
        /// Filters the brand list based on user input in the search box while preserving selections.
        /// Temporarily detaches the selection event handler during the update process.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="textChangedEventArgs">Event data for the text changed event.</param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            string searchQuery = this.SearchBox.Text.ToLower();
            List<Brand> filteredBrands = this.originalBrands
                .Where(brand => brand.BrandName.ToLower().Contains(searchQuery))
                .ToList();

            this.BrandList.SelectionChanged -= this.BrandListView_SelectionChanged;

            this.CurrentBrands.Clear();
            foreach (Brand brand in filteredBrands)
            {
                this.CurrentBrands.Add(brand);
            }

            this.BrandList.SelectedItems.Clear();
            foreach (Brand brand in filteredBrands)
            {
                if (this.selectedBrands.Contains(brand))
                {
                    this.BrandList.SelectedItems.Add(brand);
                }
            }

            this.BrandList.SelectionChanged += this.BrandListView_SelectionChanged;
        }
    }
}