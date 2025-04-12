using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WinUIApp.Models;

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class BrandFilterComponent : UserControl
    {
        private List<Brand> originalBrands = new List<Brand>();
        private HashSet<Brand> selectedBrands = new HashSet<Brand>();
        public ObservableCollection<Brand> CurrentBrands { get; set; } = new ObservableCollection<Brand>();

        /// <summary>
        /// Event that fires when the selected brands change, providing a list of selected brand names.
        /// </summary>
        public event EventHandler<List<string>> BrandChanged;

        /// <summary>
        /// Initializes a new instance of the BrandFilterComponent control.
        /// </summary>
        public BrandFilterComponent()
        {
            this.InitializeComponent();
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
                selectedBrands.Remove(removedBrand);
            }
            foreach (Brand addedBrand in selectionChangedEventArgs.AddedItems)
            {
                selectedBrands.Add(addedBrand);
            }
            BrandChanged?.Invoke(this, selectedBrands.Select(brand => brand.BrandName).ToList());
        }

        /// <summary>
        /// Filters the brand list based on user input in the search box while preserving selections.
        /// Temporarily detaches the selection event handler during the update process.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="textChangedEventArgs">Event data for the text changed event.</param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            string searchQuery = SearchBox.Text.ToLower();
            List<Brand> filteredBrands = originalBrands
                .Where(brand => brand.BrandName.ToLower().Contains(searchQuery))
                .ToList();

            BrandList.SelectionChanged -= BrandListView_SelectionChanged;

            CurrentBrands.Clear();
            foreach (Brand brand in filteredBrands)
            {
                CurrentBrands.Add(brand);
            }

            BrandList.SelectedItems.Clear();
            foreach (Brand brand in filteredBrands)
            {
                if (selectedBrands.Contains(brand))
                {
                    BrandList.SelectedItems.Add(brand);
                }
            }

            BrandList.SelectionChanged += BrandListView_SelectionChanged;
        }

        /// <summary>
        /// Updates the available brands for filtering and restores any previous selections
        /// that are still valid with the new brand list.
        /// </summary>
        /// <param name="brands">The collection of brands to be used for filtering.</param>
        public void SetBrandFilter(IEnumerable<Brand> brands)
        {
            originalBrands = brands?.ToList() ?? new List<Brand>();
            CurrentBrands.Clear();
            foreach (Brand brand in originalBrands)
            {
                CurrentBrands.Add(brand);
            }

            BrandList.SelectedItems.Clear();
            foreach (Brand brand in CurrentBrands)
            {
                if (selectedBrands.Contains(brand))
                {
                    BrandList.SelectedItems.Add(brand);
                }
            }
        }

        /// <summary>
        /// Clears all selected brands and triggers the BrandChanged event with an empty list.
        /// </summary>
        public void ClearSelection()
        {
            BrandList.SelectedItems.Clear();
            selectedBrands.Clear();
            BrandChanged?.Invoke(this, new List<string>());
        }
    }
}