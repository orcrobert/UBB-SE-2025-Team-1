using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using WinUIApp.Models;
using WinUIApp.Utils;
using WinUIApp.ViewModels;
using WinUIApp.Views.Components.SearchPageComponents;

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
            _searchPageViewModel = new SearchPageViewModel(new Services.DrinkService(), new Services.DummyServies.ReviewService());
            SortSelectorControl.SetSortOrder(_searchPageViewModel.IsAscending);
            /// trebuie facuta clasa wrapper
            if (e.Parameter is SearchPageNavigationParameters parameters)
            {
                if (parameters.InitialCategories != null)
                {
                    _searchPageViewModel.SetCategoryFilter(parameters.InitialCategories);
                }
                if (parameters.SearchedTerm != null)
                {
                    _searchPageViewModel.SetSearchedTerms(parameters.SearchedTerm);
                }
            }
            LoadDrinks();
            LoadCategoriesFilter();
            LoadBrandsFilter();
        }

        //Filter Button
        private void FilterButtonClick(object sender, RoutedEventArgs e)
        {
            LoadDrinks();
        }

        private void ClearFiltersClick(object sender, RoutedEventArgs e)
        {
            _searchPageViewModel.ClearFilters();
            CategoryFilterControl.ClearSelection();
            BrandFilterControl.ClearSelection();
            AlcoholContentFilterControl.ResetSliders();
            RatingFilterControl.ClearSelection();
            //Clear searched term
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

        // Category filter
        private void LoadCategoriesFilter()
        {
            IEnumerable<Category> categories = _searchPageViewModel.GetCategories();
            CategoryFilterControl.SetCategoriesFilter(categories);
        }

        private void CategoryFilterControl_CategoryChanged(object sender, List<string> categories)
        {
            _searchPageViewModel.SetCategoryFilter(categories);
        }

        // Brand filter

        private void LoadBrandsFilter()
        {
            IEnumerable<Brand> brands = _searchPageViewModel.GetBrands();
            BrandFilterControl.SetBrandFilter(brands);
        }

        private void BrandFilterControl_BrandChanged(object sender, List<string> brands)
        {
            _searchPageViewModel.SetBrandFilter(brands);
        }

        // Alcohol filter

        private void AlcoholContentFilterControl_MinimumAlcoholContentChanged(object sender, double minimumAlcoholContent)
        {
            float minAlcoholContent = (float)minimumAlcoholContent;
            _searchPageViewModel.SetMinAlcoholFilter(minAlcoholContent);
        }

        private void AlcoholContentFilterControl_MaximumAlcoholContentChanged(object sender, double maximumAlcoholContent)
        {
            float maxAlcoholContent = (float)maximumAlcoholContent;
            _searchPageViewModel.SetMaxAlcoholFilter(maxAlcoholContent);
        }

        // Rating Filter
        private void RatingFilterControl_RatingChanged(object sender, float rating)
        {
            _searchPageViewModel.SetMinRatingFilter(rating);
        }
    }
}