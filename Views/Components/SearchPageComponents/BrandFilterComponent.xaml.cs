using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using WinUIApp.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class BrandFilterComponent : UserControl
    {
        private IEnumerable<Brand> Brands { get; set; }

        public event EventHandler<List<string>> BrandChanged;

        public BrandFilterComponent()
        {
            this.InitializeComponent();
        }

        public void BrandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrandComboBox.SelectedItem is Brand selectedItem)
            {
                List<string> categoryFields = [];
                categoryFields.Add(selectedItem.Name);
                BrandChanged?.Invoke(this, categoryFields);
            }
        }

        public void SetBrandFilter(IEnumerable<Brand> brands)
        {
            Brands = brands;
            BrandComboBox.ItemsSource = Brands;
        }
    }
}
