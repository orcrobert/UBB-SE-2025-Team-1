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
        private HeaderViewModel _viewModel;
        public Header()
        {
            this.InitializeComponent();
            _viewModel = new HeaderViewModel(new Services.DrinkService());
            CategoryMenu.PopulateCategories(_viewModel.GetCategories());
        }

        private void Logo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AppMainFrame.Navigate(typeof(MainPage));
        }

        private void SearchDrinksButton_Click(object sender, RoutedEventArgs e)
        {
            SearchPageNavigationParameters parameters = new SearchPageNavigationParameters
            {
                SelectedCategoryFilters = CategoryMenu.SelectedCategories.Select(category => category).ToList(),
                InputSearchKeyword = DrinkSearchBox.Text
            };
            MainWindow.AppMainFrame.Navigate(typeof(SearchPage), parameters);
        }
    }
}
