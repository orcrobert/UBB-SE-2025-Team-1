using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServices;
using WinUIApp.ViewModels;

namespace WinUIApp.Views.Components
{
    public sealed partial class AddDrinkFlyout : UserControl
    {
        private AddDrinkMenuViewModel _viewModel;

        private const object defaultObjectValue = null;

        public int UserId { get; set; }

        public AddDrinkFlyout()
        {
            this.InitializeComponent();
            this.Loaded += AddDrinkFlyout_Loaded;
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

        private void AddDrinkFlyout_Loaded(object sender, RoutedEventArgs eventArguments)
        {
            var drinkService = new DrinkService();
            var userService = new UserService();
            var adminService = new AdminService();
            bool isAdmin = adminService.IsAdmin(UserId);

            var allBrands = drinkService.GetDrinkBrandNames();
            var allCategories = drinkService.GetDrinkCategories();

            _viewModel = new AddDrinkMenuViewModel(
                drinkService,
                userService,
                adminService
            )
            {
                AllBrands = allBrands,
                AllCategoryObjects = allCategories,
                AllCategories = allCategories.Select(category => category.CategoryName).ToList()
            };

            this.DataContext = _viewModel;

            if(isAdmin)
            {
                SaveButton.Content = "Add Drink";
            }
            else
            {
                SaveButton.Content = "Send Request to Admin";
            }

            CategoryList.ItemsSource = _viewModel.AllCategories;
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
            try
            {
                _viewModel.ValidateUserDrinkInput();

                var adminService = new AdminService();
                bool isAdmin = adminService.IsAdmin(UserId);

                string message;

                if (isAdmin)
                {
                    _viewModel.InstantAddDrink();
                    message = "Drink added successfully.";
                }
                else
                {
                    _viewModel.SendAddDrinkRequest();
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

                _viewModel.ClearForm();
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