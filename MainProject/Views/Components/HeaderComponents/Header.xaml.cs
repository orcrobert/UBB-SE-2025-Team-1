using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;
using WinUIApp.Utils.NavigationParameters;
using WinUIApp.Views.Pages;
using WinUIApp.Views.ViewModels;

namespace WinUIApp.Views.Components.HeaderComponents
{
    public sealed partial class Header : UserControl
    {
        private readonly HeaderViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the Header control, sets up the view model,
        /// and populates the category menu with available categories.
        /// </summary>
        public Header()
        {
            this.InitializeComponent();
            viewModel = new HeaderViewModel(new Services.DrinkService());
            CategoryMenu.PopulateCategories(viewModel.GetCategories());
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

        // Update the property names to match the correct ones in SearchPageNavigationParameters
        private void SearchDrinksButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            SearchPageNavigationParameters navigationParameters = new SearchPageNavigationParameters
            {
                SelectedCategoryFilters = CategoryMenu.SelectedCategories.ToList(), // Corrected property name
                InputSearchKeyword = DrinkSearchBox.Text // Corrected property name
            };
            MainWindow.AppMainFrame.Navigate(typeof(SearchPage), navigationParameters);
        }
    }
}
