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
using WinUIApp.Services.DummyServices;
using WinUIApp.ViewModels;

// <copyright file="UpdateDrinkFlyout.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.UI;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using Org.BouncyCastle.Asn1.Ocsp;
    using WinUIApp.Models;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServies;
    using WinUIApp.ViewModels;

    /// <summary>
    /// A flyout control for updating a drink. It includes fields for entering the drink's name, image URL, brand, alcohol content, and categories.
    /// </summary>
    public sealed partial class UpdateDrinkFlyout : UserControl
    {
        private UpdateDrinkMenuViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDrinkFlyout"/> class.
        /// </summary>
        public UpdateDrinkFlyout()
        {
            this.InitializeComponent();
            this.Loaded += this.UpdateDrinkFlyout_Loaded;
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
        /// Gets or sets the drink to be updated. This property is used to bind the drink object to the flyout.
        /// </summary>
        public Drink DrinkToUpdate { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who is updating the drink. This is used to determine if the user is an admin or not.
        /// </summary>
        public int UserId { get; set; }

        private void UpdateDrinkFlyout_Loaded(object sender, RoutedEventArgs eventArguments)
        {
            var drinkService = new DrinkService();
            var userService = new UserService();
            var adminService = new AdminService();
            bool isAdmin = adminService.IsAdmin(this.UserId);

            var allBrands = drinkService.GetDrinkBrandNames();
            var allCategories = drinkService.GetDrinkCategories();

            this.viewModel = new UpdateDrinkMenuViewModel(
                this.DrinkToUpdate,
                drinkService,
                userService,
                adminService)
            {
                AllBrands = allBrands,
                AllCategoryObjects = allCategories,
                AllCategories = allCategories.Select(category => category.CategoryName).ToList(),
                BrandName = this.DrinkToUpdate.DrinkBrand?.BrandName ?? string.Empty,
            };

            foreach (var category in this.DrinkToUpdate.CategoryList)
            {
                this.viewModel.SelectedCategoryNames.Add(category.CategoryName);
            }

            this.DataContext = this.viewModel;

            if (isAdmin)
            {
                this.SaveButton.Content = "Save";
            }
            else
            {
                this.SaveButton.Content = "Send Request to Admin";
            }

            this.CategoryList.ItemsSource = this.viewModel.AllCategories;

            foreach (var category in this.viewModel.AllCategories)
            {
                if (this.viewModel.SelectedCategoryNames.Contains(category))
                {
                    this.CategoryList.SelectedItems.Add(category);
                }
            }
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
            if (this.DrinkToUpdate == null)
            {
                return;
            }

            try
            {
                this.viewModel.ValidateUpdatedDrinkDetails();
                this.DrinkToUpdate.CategoryList = this.viewModel.GetSelectedCategories();

                var adminService = new WinUIApp.Services.DummyServices.AdminService();
                bool isAdmin = adminService.IsAdmin(UserId);

                string message;

                if (isAdmin)
                {
                    this.viewModel.InstantUpdateDrink();
                    message = "Drink updated successfully.";
                }
                else
                {
                    this.viewModel.SendUpdateDrinkRequest();
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