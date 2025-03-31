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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components
{
    public sealed partial class UpdateDrinkFlyout : UserControl
    {
        private List<string> _allCategories = new()
        {
            "Beer", "Wine", "Whiskey", "Vodka", "Cocktail", "Juice", "Cider", "Soft Drink"
        };

        public UpdateDrinkFlyout()
        {
            this.InitializeComponent();
            CategoryList.ItemsSource = new List<string>(_allCategories);

            SearchBox.TextChanged += (s, e) =>
            {
                var query = SearchBox.Text.ToLower();
                var selected = CategoryList.SelectedItems.Cast<string>().ToList();

                var filtered = _allCategories
                    .Where(c => c.ToLower().Contains(query))
                    .ToList();

                CategoryList.ItemsSource = filtered;

                foreach (var item in selected)
                    if (filtered.Contains(item))
                        CategoryList.SelectedItems.Add(item);
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = CategoryList.SelectedItems.Cast<string>().ToList();
            var dialog = new ContentDialog
            {
                Title = "Selected Categories",
                Content = string.Join(", ", selected),
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            _ = dialog.ShowAsync();
        }
    }
}
