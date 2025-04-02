using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;

namespace WinUIApp.Views.Components
{
    public sealed partial class UpdateDrinkFlyout : UserControl
    {
        public Drink DrinkToUpdate { get; set; }
        public int UserId { get; set; }

        private List<string> _allCategories = new();

        private readonly HashSet<string> _selectedCategoryNames = new();

        public UpdateDrinkFlyout()
        {
            this.InitializeComponent();

            CategoryList.ItemsSource = new List<string>(_allCategories);
            CategoryList.SelectionMode = ListViewSelectionMode.Multiple;

            CategoryList.SelectionChanged += CategoryList_SelectionChanged;

            SearchBox.TextChanged += (s, e) =>
            {
                string query = SearchBox.Text.ToLower();

                var filtered = _allCategories
                    .Where(c => c.ToLower().Contains(query))
                    .ToList();

                CategoryList.SelectionChanged -= CategoryList_SelectionChanged;

                CategoryList.ItemsSource = filtered;

                DispatcherQueue.TryEnqueue(() =>
                {
                    foreach (var item in filtered)
                    {
                        if (_selectedCategoryNames.Contains(item))
                        {
                            CategoryList.SelectedItems.Add(item);
                        }
                    }

                    CategoryList.SelectionChanged += CategoryList_SelectionChanged;
                });
            };

            this.Loaded += UpdateDrinkFlyout_Loaded;
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.Cast<string>())
                _selectedCategoryNames.Remove(removed);

            foreach (var added in e.AddedItems.Cast<string>())
                _selectedCategoryNames.Add(added);
        }

        private void UpdateDrinkFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            var adminService = new WinUIApp.Services.DummyServies.AdminService();
            var service = new WinUIApp.Services.DrinkService();
            bool isAdmin = adminService.IsAdmin(UserId);
            _allCategories = _allCategories = service.getDrinkCategories().Select(c => c.Name).ToList();

            SaveButton.Content = isAdmin ? "Save" : "Send Request to Admin";

            if (DrinkToUpdate != null)
            {
                BrandBox.Text = DrinkToUpdate.Brand?.Name ?? "";
                AlcoholBox.Text = DrinkToUpdate.AlcoholContent.ToString();
                NameBox.Text = DrinkToUpdate.DrinkName;
                ImageUrlBox.Text = DrinkToUpdate.DrinkURL;

                var selectedNames = DrinkToUpdate.Categories.Select(c => c.Name).ToList();
                foreach (var name in selectedNames)
                {
                    _selectedCategoryNames.Add(name);
                }

                CategoryList.ItemsSource = _allCategories;

                foreach (var item in _allCategories)
                {
                    if (_selectedCategoryNames.Contains(item))
                        CategoryList.SelectedItems.Add(item);
                }
            }
        }

        private Brand ResolveBrand(string brandName)
        {
            var service = new DrinkService();
            var existingBrands = service.getDrinkBrands();

            var match = existingBrands
                .FirstOrDefault(b => b.Name.Equals(brandName, StringComparison.OrdinalIgnoreCase));

            ///Daca nu exista brand-ul, dam eroare. Brandul trebuie sa existe.
            if (match == null)
            {
                throw new ArgumentException("The brand you tried to add was not found.");
            }
            return match;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrinkToUpdate != null)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(NameBox.Text))
                    {
                        throw new ArgumentException("Drink name is required");
                    }

                    if (string.IsNullOrWhiteSpace(BrandBox.Text))
                    {
                        throw new ArgumentException("Brand is required");
                    }

                    if (!float.TryParse(AlcoholBox.Text, out var alcoholContent) || alcoholContent < 0 || alcoholContent > 100)
                    {
                        throw new ArgumentException("Valid alcohol content (0-100%) is required");
                    }

                    if (_selectedCategoryNames.Count == 0)
                    {
                        throw new ArgumentException("At least one category must be selected");
                    }

                    DrinkToUpdate.Brand = ResolveBrand(BrandBox.Text);
                    DrinkToUpdate.DrinkName = NameBox.Text;
                    DrinkToUpdate.DrinkURL = ImageUrlBox.Text;

                    if (float.TryParse(AlcoholBox.Text, out var alc))
                        DrinkToUpdate.AlcoholContent = alc;

                    DrinkToUpdate.Categories = _selectedCategoryNames
                        .Select(name => new Category(1, name))
                        .ToList();

                    var adminService = new WinUIApp.Services.DummyServies.AdminService();
                    bool isAdmin = adminService.IsAdmin(UserId);

                    string message;

                    if (isAdmin)
                    {
                        var service = new DrinkService();
                        Debug.WriteLine("IMPORTANT Categories: " + string.Join(", ", DrinkToUpdate.Categories.Select(c => c.Name)));
                        service.updateDrink(DrinkToUpdate);
                        var testDrink = service.getDrinks(null, null, null, null, null, null)[0];
                        Debug.WriteLine("IMPORTANT Categories: " + string.Join(", ", DrinkToUpdate.Categories.Select(c => c.Name)));
                        message = "Drink updated successfully.";
                    }
                    else
                    {
                        message = "A request was sent to the admin.";
                        adminService.SendNotification(
                            senderUserID: UserId,
                            title: "Drink Update Request",
                            description: $"User requested to update drink: {DrinkToUpdate.DrinkName}"
                        );
                    }

                    var dialog = new ContentDialog
                    {
                        Title = "Success",
                        Content = message,
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = ex.Message,
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
        }

    }
}