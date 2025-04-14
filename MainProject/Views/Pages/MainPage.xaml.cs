// <copyright file="MainPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Pages
{
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Navigation;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;
    using WinUIApp.ViewModels;

    /// <summary>
    /// MainPage.xaml's code-behind file.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.viewModel = new MainPageViewModel(new DrinkService(), new UserService());
            this.DataContext = this.viewModel;
        }

        /// <summary>
        /// Handles the navigation to this page. Sets the previous page in the MainWindow to this page.
        /// </summary>
        /// <param name="eventArguments">Event Arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs eventArguments)
        {
            base.OnNavigatedTo(eventArguments);
            MainWindow.PreviousPage = typeof(MainPage);
        }

        /// <summary>
        /// Handles the Tapped event for the DrinkOfTheDayComponent. Navigates to the DrinkDetailPage with the drink of the day ID.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="eventArguments">Event Arguments.</param>
        private void DrinkOfTheDayComponent_Tapped(object sender, TappedRoutedEventArgs eventArguments)
        {
            MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), this.viewModel.GetDrinkOfTheDayId());
        }
    }
}