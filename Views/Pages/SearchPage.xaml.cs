using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using WinUIApp.Models;
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
                SortSelectorControl.SetSortOrder(_searchPageViewModel.IsAscending);
                LoadDrinks();
                LoadCategoriesFilter();
            }
        }

        //Filter Button
        private void FilterButtonClick(object sender, RoutedEventArgs e)
        {
            LoadDrinks();
        }

        //DrinkList

        private void VerticalDrinkListControl_DrinkClicked(object sender, int drinkId)
        {
            _searchPageViewModel.OpenDrinkDetailPage(drinkId);
        }


        private void LoadDrinks()
        {
            IEnumerable<DrinkDisplayItem> drinks = _searchPageViewModel.GetDrinks();
            VerticalDrinkListControl.SetDrinks(drinks);
        }

        //Sort

        private void SortByDropdownControl_SortByChanged(object sender, string sortField)
        {
            _searchPageViewModel.SetSortByField(sortField);
            LoadDrinks();
        }

        private void SortSelectorControl_SortOrderChanged(object sender, bool isAscending)
        {
            _searchPageViewModel.SetSortOrder(isAscending);
            LoadDrinks();
        }

        /// Category filter
        private void LoadCategoriesFilter()
        {
            IEnumerable<Category> categories = _searchPageViewModel.GetCategories();
            CategoryFilterControl.SetCategoriesFilter(categories);
        }

        private void CategoryFilterControl_CategoryChanged(object sender, List<string> categories)
        {
            _searchPageViewModel.SetCategoryFilter(categories);
        }
    }
}