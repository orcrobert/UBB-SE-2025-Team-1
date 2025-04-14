// <copyright file="Header.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components.HeaderComponents
{
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.Utils.NavigationParameters;
    using WinUIApp.Views.Pages;
    using WinUIApp.Views.ViewModels;

    /// <summary>
    /// Represents the header component of the application, including the logo, search box, and category menu.
    /// </summary>
    public sealed partial class Header : UserControl
    {
        private readonly HeaderViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        public Header()
        {
            this.InitializeComponent();
            this.viewModel = new HeaderViewModel(new Services.DrinkService());
            this.CategoryMenu.PopulateCategories(this.viewModel.GetCategories());
        }

        /// <summary>
        /// Handles click events on the logo by navigating to the MainPage.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="routedEventArgs">Event data for the click event.</param>
        private void Logo_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.AppMainFrame.Navigate(typeof(MainPage));
        }

        private void SearchDrinksButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            SearchPageNavigationParameters navigationParameters = new SearchPageNavigationParameters
            {
                SelectedCategoryFilters = this.CategoryMenu.SelectedCategories.ToList(),
                InputSearchKeyword = this.DrinkSearchBox.Text,
            };
            MainWindow.AppMainFrame.Navigate(typeof(SearchPage), navigationParameters);
        }
    }
}