using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class BrandFilterComponent : UserControl
    {
        private IEnumerable<Brand> Brands { get; set; }
        private IEnumerable<Brand> FilteredBrands { get; set; }

        private List<string> _brandSelectedFields = [];

        public event EventHandler<List<string>> BrandChanged;

        public BrandFilterComponent()
        {
            this.InitializeComponent();
        }

        private void BrandListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.Cast<Brand>())
                _brandSelectedFields.Remove(removed.Name);

            foreach (var added in e.AddedItems.Cast<Brand>())
                _brandSelectedFields.Add(added.Name);


            BrandChanged?.Invoke(this, _brandSelectedFields);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            var selectedItems = BrandList.SelectedItems.Cast<Brand>().ToList();

            FilteredBrands = Brands.Where(c => c.Name.ToLower().Contains(query)).Cast<Brand>();
            BrandList.ItemsSource = FilteredBrands;

            foreach (var item in selectedItems)
            {
                if (FilteredBrands.Contains(item))
                    BrandList.SelectedItems.Add(item);
            }
        }


        public void SetBrandFilter(IEnumerable<Brand> brands)
        {
            Brands = brands;
            FilteredBrands = Brands;
            BrandList.ItemsSource = FilteredBrands;
        }

        public void ClearSelection()
        {
            BrandList.SelectedItem = null;
        }
    }
}
