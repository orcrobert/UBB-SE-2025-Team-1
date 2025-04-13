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

// <copyright file="AddDrinkFlyout.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.UI;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using WinUIApp.Models;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;
    using WinUIApp.ViewModels;

    /// <summary>
    /// A flyout control for adding a new drink. It includes fields for entering the drink's name, image URL, brand, alcohol content, and categories.
    /// </summary>
    public sealed partial class AddDrinkFlyout : UserControl
    {
        private AddDrinkMenuViewModel viewModel;

        private readonly AdminService adminService;
        /// <summary>
        /// Initializes a new instance of the <see cref="AddDrinkFlyout"/> class.
        /// </summary>
        public AddDrinkFlyout()
        {
            adminService = new AdminService();
            this.InitializeComponent();
            this.Loaded += this.AddDrinkFlyout_Loaded;
            this.CategoryList.SelectionChanged += this.CategoryList_SelectionChanged;

            this.SearchBox.TextChanged += (sender, eventArguments) =>
            {
                string query = this.SearchBox.Text.ToLower();

                var filteredCategories = this.viewModel.AllCategories
                    .Where(category => category.ToLower().Contains(query))
                    .ToList();

                this.CategoryList.SelectionChanged -= this.CategoryList_SelectionChanged;
                this.CategoryList.ItemsSource = filteredCategories;

                this.DispatcherQueue.TryEnqueue(() =>
                {
                    foreach (var category in filteredCategories)
                    {
                        if (this.viewModel.SelectedCategoryNames.Contains(category))
                        {
                            this.CategoryList.SelectedItems.Add(category);
                        }
                    }

                    this.CategoryList.SelectionChanged += this.CategoryList_SelectionChanged;
                });
            };
        }

        /// <summary>
        /// Gets or sets for the ID of the user who is adding the drink. This is used to determine if the user is an admin or not.
        /// </summary>
        public int UserId { get; set; }

        private void AddDrinkFlyout_Loaded(object sender, RoutedEventArgs eventArguments)
        {
            var drinkService = new DrinkService();
            var userService = new UserService();
            bool isAdmin = adminService.IsAdmin(this.UserId);

            var allBrands = drinkService.GetDrinkBrandNames();
            var allCategories = drinkService.GetDrinkCategories();

            this.viewModel = new AddDrinkMenuViewModel(
                drinkService,
                userService,
                adminService)
            {
                AllBrands = allBrands,
                AllCategoryObjects = allCategories,
                AllCategories = allCategories.Select(category => category.CategoryName).ToList(),
            };

            this.DataContext = this.viewModel;

            if (isAdmin)
            {
                this.SaveButton.Content = "Add Drink";
            }
            else
            {
                this.SaveButton.Content = "Send Request to Admin";
            }

            this.CategoryList.ItemsSource = this.viewModel.AllCategories;
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs eventArguments)
        {
            if (this.viewModel == null)
            {
                return;
            }

            foreach (var removedCategory in eventArguments.RemovedItems.Cast<string>())
            {
                this.viewModel.SelectedCategoryNames.Remove(removedCategory);
            }

            foreach (var addedCategory in eventArguments.AddedItems.Cast<string>())
            {
                if (!this.viewModel.SelectedCategoryNames.Contains(addedCategory))
                {
                    this.viewModel.SelectedCategoryNames.Add(addedCategory);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            try
            {

                this.viewModel.ValidateUserDrinkInput();

                bool isAdmin = adminService.IsAdmin(this.UserId);

                string message;

                if (isAdmin)
                {
                    this.viewModel.InstantAddDrink();
                    message = "Drink added successfully.";
                }
                else
                {
                    this.viewModel.SendAddDrinkRequest();
                    message = "A request was sent to the admin.";
                }

                var dialog = new ContentDialog
                {
                    Title = "Success",
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot,
                };
                _ = dialog.ShowAsync();

                this.viewModel.ClearForm();
            }
            catch (Exception exception)
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = exception.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot,
                };
                _ = dialog.ShowAsync();
            }
        }
    }
}