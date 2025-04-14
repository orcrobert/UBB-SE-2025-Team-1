// <copyright file="SearchPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Pages
{
    using System.Collections.Generic;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using WinUIApp.Models;
    using WinUIApp.Utils.NavigationParameters;
    using WinUIApp.ViewModels;
    using WinUIApp.Views.Components.SearchPageComponents;

    /// <summary>
    /// SearchPage.xaml's interaction logic.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private SearchPageViewModel searchPageViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPage"/> class.
        /// </summary>
        public SearchPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the navigation to this page.
        /// </summary>
        /// <param name="eventArguments">Event arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs eventArguments)
        {
            base.OnNavigatedTo(eventArguments);
            MainWindow.PreviousPage = typeof(SearchPage);
            this.searchPageViewModel = new SearchPageViewModel(new Services.DrinkService(), new Services.DummyServices.DrinkReviewService());
            this.SortSelectorControl.SetSortOrder(this.searchPageViewModel.IsAscending);
            if (eventArguments.Parameter is SearchPageNavigationParameters parameters)
            {
                if (parameters.SelectedCategoryFilters != null)
                {
                    this.searchPageViewModel.SetInitialCategoryFilter(parameters.SelectedCategoryFilters);
                }

                if (parameters.InputSearchKeyword != null)
                {
                    this.searchPageViewModel.SetSearchedTerms(parameters.InputSearchKeyword);
                }
            }

            this.LoadDrinks();
            this.LoadCategoriesFilter();
            this.LoadBrandsFilter();
        }

        private void FilterButtonClick(object sender, RoutedEventArgs eventArguments)
        {
            this.LoadDrinks();
        }

        private void ClearFiltersClick(object sender, RoutedEventArgs eventArguments)
        {
            this.searchPageViewModel.ClearFilters();
            this.CategoryFilterControl.ClearSelection();
            this.BrandFilterControl.ClearSelection();
            this.AlcoholContentFilterControl.ResetSliders();
            this.RatingFilterControl.ClearSelection();
            this.LoadDrinks();
        }

        private void VerticalDrinkListControl_DrinkClicked(object sender, int drinkId)
        {
            this.searchPageViewModel.OpenDrinkDetailPage(drinkId);
        }

        private void LoadDrinks()
        {
            IEnumerable<DrinkDisplayItem> drinks = this.searchPageViewModel.GetDrinks();
            this.VerticalDrinkListControl.SetDrinks(drinks);
        }

        private void SortByDropdownControl_SortByChanged(object sender, string sortField)
        {
            this.searchPageViewModel.SetSortByField(sortField);
            this.LoadDrinks();
        }

        private void SortSelectorControl_SortOrderChanged(object sender, bool isAscending)
        {
            this.searchPageViewModel.SetSortOrder(isAscending);
            this.LoadDrinks();
        }

        private void LoadCategoriesFilter()
        {
            IEnumerable<Category> categories = this.searchPageViewModel.GetCategories();
            IEnumerable<Category> initialCategories = this.GetInitialCategories();
            this.CategoryFilterControl.SetCategoriesFilter(categories, initialCategories);
        }

        private void CategoryFilterControl_CategoryChanged(object sender, List<string> categories)
        {
            this.searchPageViewModel.SetCategoryFilter(categories);
        }

        private List<Category> GetInitialCategories()
        {
            return this.searchPageViewModel.InitialCategories;
        }

        private void LoadBrandsFilter()
        {
            IEnumerable<Brand> brands = this.searchPageViewModel.GetBrands();
            this.BrandFilterControl.SetBrandFilter(brands);
        }

        private void BrandFilterControl_BrandChanged(object sender, List<string> brands)
        {
            this.searchPageViewModel.SetBrandFilter(brands);
        }

        private void AlcoholContentFilterControl_MinimumAlcoholContentChanged(object sender, double minimumAlcoholContent)
        {
            float minAlcoholContent = (float)minimumAlcoholContent;
            this.searchPageViewModel.SetMinAlcoholFilter(minAlcoholContent);
        }

        private void AlcoholContentFilterControl_MaximumAlcoholContentChanged(object sender, double maximumAlcoholContent)
        {
            float maxAlcoholContent = (float)maximumAlcoholContent;
            this.searchPageViewModel.SetMaxAlcoholFilter(maxAlcoholContent);
        }

        private void RatingFilterControl_RatingChanged(object sender, float rating)
        {
            this.searchPageViewModel.SetMinRatingFilter(rating);
        }
    }
}