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
        private List<Brand> _originalBrands = new List<Brand>();
        private HashSet<Brand> _selectedBrands = new HashSet<Brand>();
        public ObservableCollection<Brand> CurrentBrands { get; set; } = new ObservableCollection<Brand>();

        public event EventHandler<List<string>> BrandChanged;

        public BrandFilterComponent()
        {
            this.InitializeComponent();
        }

        private void BrandListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Brand removedBrand in e.RemovedItems)
                _selectedBrands.Remove(removedBrand);

            foreach (Brand addedBrand in e.AddedItems)
                _selectedBrands.Add(addedBrand);

            BrandChanged?.Invoke(this, _selectedBrands.Select(b => b.Name).ToList());
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();

            List<Brand> filteredBrands = _originalBrands
                .Where(brand => brand.Name.ToLower().Contains(query))
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
                if (_selectedBrands.Contains(brand))
                {
                    BrandList.SelectedItems.Add(brand);
                }
            }

            BrandList.SelectionChanged += BrandListView_SelectionChanged;
        }

        public void SetBrandFilter(IEnumerable<Brand> brands)
        {
            _originalBrands = brands?.ToList() ?? new List<Brand>();
            CurrentBrands.Clear();
            foreach (Brand brand in _originalBrands)
            {
                CurrentBrands.Add(brand);
            }

            BrandList.SelectedItems.Clear();
            foreach (Brand brand in CurrentBrands)
            {
                if (_selectedBrands.Contains(brand))
                {
                    BrandList.SelectedItems.Add(brand);
                }
            }
        }

        public void ClearSelection()
        {
            BrandList.SelectedItems.Clear();
            _selectedBrands.Clear();
            BrandChanged?.Invoke(this, new List<string>());
        }
    }
}