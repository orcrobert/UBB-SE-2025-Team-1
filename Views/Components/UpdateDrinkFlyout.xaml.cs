using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.ViewModels;

namespace WinUIApp.Views.Components
{
    public sealed partial class UpdateDrinkFlyout : UserControl
    {
        private UpdateDrinkMenuViewModel _viewModel;

        public Drink DrinkToUpdate { get; set; }
        public int UserId { get; set; }

        public UpdateDrinkFlyout()
        {
            this.InitializeComponent();
            this.Loaded += UpdateDrinkFlyout_Loaded;
            CategoryList.SelectionChanged += CategoryList_SelectionChanged;

            SearchBox.TextChanged += (s, e) =>
            {
                string query = SearchBox.Text.ToLower();

                var filtered = _viewModel.AllCategories
                    .Where(c => c.ToLower().Contains(query))
                    .ToList();

                CategoryList.SelectionChanged -= CategoryList_SelectionChanged;
                CategoryList.ItemsSource = filtered;

                DispatcherQueue.TryEnqueue(() =>
                {
                    foreach (var item in filtered)
                    {
                        if (_viewModel.SelectedCategoryNames.Contains(item))
                            CategoryList.SelectedItems.Add(item);
                    }

                    CategoryList.SelectionChanged += CategoryList_SelectionChanged;
                });
            };
        }

        private void UpdateDrinkFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            var drinkService = new DrinkService();
            var userService = new UserService();
            var adminService = new AdminService();
            bool isAdmin = adminService.IsAdmin(UserId);

            var allBrands = drinkService.GetDrinkBrandNames(); 
            var allCategories = drinkService.GetDrinkCategories();

            _viewModel = new UpdateDrinkMenuViewModel(
                DrinkToUpdate,
                drinkService,
                userService,
                adminService
            )
            {
                AllBrands = allBrands,
                AllCategoryObjects = allCategories,
                AllCategories = allCategories.Select(c => c.Name).ToList(),
                BrandName = DrinkToUpdate.Brand?.Name ?? ""
            };

            foreach (var category in DrinkToUpdate.Categories)
                _viewModel.SelectedCategoryNames.Add(category.Name);

            this.DataContext = _viewModel;

            SaveButton.Content = isAdmin ? "Save" : "Send Request to Admin";

            CategoryList.ItemsSource = _viewModel.AllCategories;

            foreach (var item in _viewModel.AllCategories)
            {
                if (_viewModel.SelectedCategoryNames.Contains(item))
                    CategoryList.SelectedItems.Add(item);
            }
        }


        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel == null) return;

            foreach (var removed in e.RemovedItems.Cast<string>())
                _viewModel.SelectedCategoryNames.Remove(removed);

            foreach (var added in e.AddedItems.Cast<string>())
                if (!_viewModel.SelectedCategoryNames.Contains(added))
                    _viewModel.SelectedCategoryNames.Add(added);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrinkToUpdate == null)
                return;

            try
            {
                _viewModel.Validate();
                DrinkToUpdate.Categories = _viewModel.GetSelectedCategories();

                var adminService = new WinUIApp.Services.DummyServies.AdminService();
                bool isAdmin = adminService.IsAdmin(UserId);

                string message;

                if (isAdmin)
                {
                    _viewModel.InstantUpdateDrink();
                    message = "Drink updated successfully.";
                }
                else
                {
                    _viewModel.SendUpdateDrinkRequest();
                    message = "A request was sent to the admin.";
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
