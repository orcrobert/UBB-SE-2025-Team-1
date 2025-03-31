using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using WinUIApp.Views.Components.SearchPageComponents;
using WinUIApp.Views.ModelViews;

namespace WinUIApp.Views.Pages
{
    public sealed partial class SearchPage : Page
    {
        private SearchPageViewModel _searchPageViewModel;

        public SearchPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is SearchPageViewModel viewModel)
            {
                _searchPageViewModel = viewModel;
                VerticalDrinkListControl.DrinkClicked += VerticalDrinkListControl_DrinkClicked;
                LoadDrinks();
            }
        }

        private void VerticalDrinkListControl_DrinkClicked(object sender, int drinkId)
        {
            _searchPageViewModel.OpenDrinkDetailPage(drinkId);
        }

        private void LoadDrinks()
        {
            IEnumerable<DrinkDisplayItem> drinks = _searchPageViewModel.GetDrinks();
            VerticalDrinkListControl.SetDrinks(drinks);
        }
    }
}