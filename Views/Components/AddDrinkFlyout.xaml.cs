using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;

namespace WinUIApp.Views.Components
{
    public sealed partial class AddDrinkFlyout : UserControl
    {
        public int UserId { get; set; }

        private readonly List<string> _allCategories = new();
        

        private readonly HashSet<string> _selectedCategoryNames = new();

        public AddDrinkFlyout()
        {
            this.InitializeComponent();
            var service = new DrinkService();
            _allCategories = service.getDrinkCategories().Select(c => c.Name).ToList();

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
        }

        //Asta e metoda care se apeleaza cand se schimba selectia de categorii
        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems.Cast<string>())
                _selectedCategoryNames.Remove(removed);

            foreach (var added in e.AddedItems.Cast<string>())
                _selectedCategoryNames.Add(added);
        }

        //Asta e metoda care verifica daca brand-ul exista deja in baza de date
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
            //Ceva mici validari. Catalin this is just for refference, you can get rid of it daca vrei sa faci tu validarea
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
                

                var brand = ResolveBrand(BrandBox.Text);
                var categories = _selectedCategoryNames
                    .Select(name => new Category(1, name))
                    .ToList();

                var adminService = new WinUIApp.Services.DummyServies.AdminService();
                bool isAdmin = adminService.IsAdmin(UserId);
                //isAdmin = true; // Doar testez aici daca merge add-u, comentati linia asta si va merge normal
                string message;
                if (isAdmin)
                {
                    var service = new DrinkService();
                    service.addDrink(
                        drinkName: NameBox.Text,
                        drinkUrl: ImageUrlBox.Text,
                        categories: categories,
                        brandName: brand.Name,
                        alcoholContent: alcoholContent
                    );
                    message = "Drink added successfully.";
                }
                else
                {
                    message = "A request was sent to the admin.";
                    adminService.SendNotification(
                        senderUserID: UserId,
                        title: "New Drink Request",
                        description: $"User requested to add new drink: {NameBox.Text}"
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

                // Clear the form
                NameBox.Text = string.Empty;
                ImageUrlBox.Text = string.Empty;
                BrandBox.Text = string.Empty;
                AlcoholBox.Text = string.Empty;
                _selectedCategoryNames.Clear();
                CategoryList.SelectedItems.Clear();
            }
            catch (Exception ex) when (ex.Message.Contains("The brand you tried to add was not found."))
            {
                // Specific pt eroarea de brand. Informam user-ul ce brnduri sunt valabile, ca sa stie pe data viitoare
                var service = new DrinkService();
                var availableBrands = service.getDrinkBrands();
                var brandList = string.Join(", ", availableBrands.Select(b => b.Name));

                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Brand does not exist. Available brands are: {brandList}",
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