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

        private const object defaultObjectValue = null;

        public Drink DrinkToUpdate { get; set; }
        public int UserId { get; set; }

        public UpdateDrinkFlyout()
        {
            this.InitializeComponent();
            this.Loaded += UpdateDrinkFlyout_Loaded;
            CategoryList.SelectionChanged += CategoryList_SelectionChanged;

            SearchBox.TextChanged += (sender, eventArguments) =>
            {
                string query = SearchBox.Text.ToLower();

                var filteredCategories = _viewModel.AllCategories
                    .Where(category => category.ToLower().Contains(query))
                    .ToList();

                CategoryList.SelectionChanged -= CategoryList_SelectionChanged;
                CategoryList.ItemsSource = filteredCategories;

                DispatcherQueue.TryEnqueue(() =>
                {
                    foreach (var category in filteredCategories)
                    {
                        if (_viewModel.SelectedCategoryNames.Contains(category))
                        {
                            CategoryList.SelectedItems.Add(category);
                        }
                    }

                    CategoryList.SelectionChanged += CategoryList_SelectionChanged;
                });
            };
        }

        private void UpdateDrinkFlyout_Loaded(object sender, RoutedEventArgs eventArguments)
        {
            var drinkService = new DrinkService();
            var userService = new UserService();
            var adminService = new AdminService();
            bool isAdmin = adminService.IsAdmin(UserId);

            var allBrands = drinkService.getDrinkBrands(); 
            var allCategories = drinkService.getDrinkCategories();

            _viewModel = new UpdateDrinkMenuViewModel(
                DrinkToUpdate,
                drinkService,
                userService,
                adminService
            )
            {
                AllBrands = allBrands,
                AllCategoryObjects = allCategories,
                AllCategories = allCategories.Select(category => category.Name).ToList(),
                BrandName = DrinkToUpdate.Brand?.Name ?? String.Empty
            };

            foreach (var category in DrinkToUpdate.Categories)
            {
                _viewModel.SelectedCategoryNames.Add(category.Name);
            }

            this.DataContext = _viewModel;

            if(isAdmin)
            {
                SaveButton.Content = "Save";
            }
            else
            {
                SaveButton.Content = "Send Request to Admin";
            }

            CategoryList.ItemsSource = _viewModel.AllCategories;

            foreach (var category in _viewModel.AllCategories)
            {
                if (_viewModel.SelectedCategoryNames.Contains(category))
                {
                    CategoryList.SelectedItems.Add(category);
                }
            }
        }


        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs eventArguments)
        {
            if (_viewModel == defaultObjectValue) return;

            foreach (var removedCategory in eventArguments.RemovedItems.Cast<string>())
            {
                _viewModel.SelectedCategoryNames.Remove(removedCategory);
            }

            foreach (var addedCategory in eventArguments.AddedItems.Cast<string>())
            {
                if (!_viewModel.SelectedCategoryNames.Contains(addedCategory))
                {
                    _viewModel.SelectedCategoryNames.Add(addedCategory);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            if (DrinkToUpdate == defaultObjectValue)
            {
                return;
            }

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
            catch (Exception exception)
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = exception.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                _ = dialog.ShowAsync();
            }
        }
    }
}
