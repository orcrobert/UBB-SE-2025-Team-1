// <copyright file="DrinkDetailPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Pages
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using WinUIApp.Models;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;
    using WinUIApp.Views.Components;
    using WinUIApp.Views.ViewModels;

    /// <summary>
    /// Represents the DrinkDetailPage, which displays detailed information about a drink.
    /// </summary>
    public sealed partial class DrinkDetailPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkDetailPage"/> class.
        /// </summary>
        public DrinkDetailPage()
        {
            this.InitializeComponent();
            this.DataContext = this.ViewModel;
            if (this.ViewModel.IsCurrentUserAdmin())
            {
                this.RemoveButtonText.Text = "Remove drink";
            }
            else
            {
                this.RemoveButtonText.Text = "Send remove drink request";
            }

            this.UpdateButton.OnDrinkUpdated = () =>
            {
                this.ViewModel.LoadDrink(this.ViewModel.Drink.DrinkId);
            };
        }

        /// <summary>
        /// Gets the view model for the DrinkDetailPage.
        /// </summary>
        public DrinkDetailPageViewModel ViewModel { get; } = new DrinkDetailPageViewModel(new Services.DrinkService(), new Services.DummyServices.DrinkReviewService(), new Services.DummyServices.UserService(), new Services.DummyServices.AdminService());

        /// <summary>
        /// Handles the navigation to the page. It loads the drink details based on the passed drink ID.
        /// </summary>
        /// <param name="eventArguments">Event arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs eventArguments)
        {
            base.OnNavigatedTo(eventArguments);
            if (eventArguments.Parameter is int drinkId)
            {
                this.ViewModel.LoadDrink(drinkId);
            }
        }

        /// <summary>
        /// Handles the click event of the back button. It navigates back to the previous page.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="eventArguments">Event arguments.</param>
        private void ConfirmRemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs eventArguments)
        {
            this.ViewModel.RemoveDrink();
            MainWindow.AppMainFrame.Navigate(MainWindow.PreviousPage);
        }

        /// <summary>
        /// Handles the click event of the vote button. It allows the user to vote for the drink.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="eventArguments">Event arguments.</param>
        private void VoteButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            this.ViewModel.VoteForDrink();
        }
    }
}