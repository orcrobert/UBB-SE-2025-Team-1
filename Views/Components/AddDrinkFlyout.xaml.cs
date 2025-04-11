using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.ViewModels;

namespace WinUIApp.Views.Components
{
    public sealed partial class AddDrinkFlyout : UserControl
    {
        private AddDrinkMenuViewModel _viewModel;

        public int UserId { get; set; }

        public AddDrinkFlyout()
        {
            this.InitializeComponent();
            this.Loaded += AddDrinkFlyout_Loaded;
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

        private void AddDrinkFlyout_Loaded(object sender, RoutedEventArgs e)
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
                AllCategories = allCategories.Select(c => c.Name).ToList()
            };

            this.DataContext = _viewModel;

            SaveButton.Content = isAdmin ? "Add Drink" : "Send Request to Admin";

            CategoryList.ItemsSource = _viewModel.AllCategories;
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
            try
            {
                _viewModel.Validate();

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