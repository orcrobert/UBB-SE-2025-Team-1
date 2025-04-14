// <copyright file="SearchPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Mysqlx.Crud;
    using WinUIApp.Models;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;
    using WinUIApp.Views;
    using WinUIApp.Views.Components.SearchPageComponents;
    using WinUIApp.Views.Pages;

    /// <summary>
    /// ViewModel for the SearchPage. Manages the display of drinks based on various filters and sorting options.
    /// </summary>
    /// <param name="drinkService">The drink service used to manage drinks.</param>
    /// <param name="reviewService">The review service used to manage reviews.</param>
    public class SearchPageViewModel(IDrinkService drinkService, IDrinkReviewService reviewService)
    {
        private const string NameField = "Name";

        private readonly IDrinkService drinkService = drinkService;
        private IDrinkReviewService reviewService = reviewService;

        private bool isAscending = true;
        private string fieldToSortBy = NameField;

        private List<string>? categoryFilter;
        private List<string>? brandFilter;
        private float? minAlcoholFilter;
        private float? maxAlcoholFilter;
        private float? minRatingFilter;
        private string? searchedTerms;

        /// <summary>
        /// Gets or sets the list of drink categories to be displayed on the page.
        /// </summary>
        public List<Category> InitialCategories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the drinks are sorted in ascending order. Default is true (ascending order).
        /// </summary>
        public bool IsAscending
        {
            get => this.isAscending;
            set => this.isAscending = value;
        }

        /// <summary>
        /// Gets or sets the field by which the drinks are sorted. Default is "Name".
        /// </summary>
        public string FieldToSortBy
        {
            get => this.fieldToSortBy;
            set => this.fieldToSortBy = value;
        }

        /// <summary>
        /// Opens the drink detail page for a specific drink ID.
        /// </summary>
        /// <param name="id">ID of the drink to be displayed.</param>
        public void OpenDrinkDetailPage(int id)
        {
            MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), id);
        }

        /// <summary>
        /// Clears all filters applied to the drink list.
        /// </summary>
        public void ClearFilters()
        {
            this.categoryFilter = null;
            this.brandFilter = null;
            this.minAlcoholFilter = null;
            this.maxAlcoholFilter = null;
            this.minRatingFilter = null;
            this.searchedTerms = null;
        }

        /// <summary>
        /// Iterates through the drinks and creates a list of DrinkDisplayItem objects representing the drinks.
        /// If the sortByField is "Name" or "Alcohol Content", it sorts the drinks accordingly.
        /// If the sortByField is not "Name" or "Alcohol Content", it sorts the drinks by average review score.
        /// If the minRatingFilter is set, it filters the drinks based on the average review score.
        /// If the minRatingFilter is not set, it includes all drinks.
        /// </summary>
        /// <returns>An enumerable collection of DrinkDisplayItem objects representing the drinks.</returns>
        public IEnumerable<DrinkDisplayItem> GetDrinks()
        {
            List<DrinkDisplayItem> displayItems = new List<DrinkDisplayItem>();

            if (this.fieldToSortBy == NameField || this.fieldToSortBy == "Alcohol Content")
            {
                string sortField;
                if (this.fieldToSortBy == NameField)
                {
                    sortField = "D.DrinkName";
                }
                else
                {
                    sortField = "D.AlcoholContent";
                }

                var orderBy = new Dictionary<string, bool>
                {
                    { sortField, this.isAscending },
                };
                List<Drink> drinks = this.drinkService.GetDrinks(
                    searchKeyword: this.searchedTerms,
                    drinkBrandNameFilter: this.brandFilter,
                    drinkCategoryFilter: this.categoryFilter,
                    minimumAlcoholPercentage: this.minAlcoholFilter,
                    maximumAlcoholPercentage: this.maxAlcoholFilter,
                    orderingCriteria: orderBy);

                displayItems = new List<DrinkDisplayItem>();
                foreach (Drink drink in drinks)
                {
                    float averageScore = this.reviewService.GetReviewAverageByID(drink.DrinkId);
                    if (this.minRatingFilter == null)
                    {
                        displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                    }
                    else
                    {
                        if (averageScore >= this.minRatingFilter)
                        {
                            displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                        }
                    }
                }
            }
            else
            {
                List<Drink> drinks = this.drinkService.GetDrinks(
                    searchKeyword: this.searchedTerms,
                    drinkBrandNameFilter: this.brandFilter,
                    drinkCategoryFilter: this.categoryFilter,
                    minimumAlcoholPercentage: this.minAlcoholFilter,
                    maximumAlcoholPercentage: this.maxAlcoholFilter,
                    orderingCriteria: null);

                displayItems = new List<DrinkDisplayItem>();
                foreach (Drink drink in drinks)
                {
                    float averageScore = this.reviewService.GetReviewAverageByID(drink.DrinkId);
                    if (this.minRatingFilter == null)
                    {
                        displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                    }
                    else
                    {
                        if (averageScore >= this.minRatingFilter)
                        {
                            displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                        }
                    }
                }

                if (this.isAscending)
                {
                    displayItems = displayItems.OrderBy(item => item.AverageReviewScore).ToList();
                }
                else
                {
                    displayItems = displayItems.OrderByDescending(item => item.AverageReviewScore).ToList();
                }
            }

            return displayItems;
        }

        /// <summary>
        /// Retrieves the list of drink categories from the drink service.
        /// </summary>
        /// <returns>The list of drink categories.</returns>
        public IEnumerable<Category> GetCategories()
        {
            return this.drinkService.GetDrinkCategories();
        }

        /// <summary>
        /// Retrieves the list of drink brands from the drink service.
        /// </summary>
        /// <returns>The list of drink brands.</returns>
        public IEnumerable<Brand> GetBrands()
        {
            return this.drinkService.GetDrinkBrandNames();
        }

        /// <summary>
        /// Sets the sorting field for the drinks. The default is "Name".
        /// </summary>
        /// <param name="sortByField">Sort parameter.</param>
        public void SetSortByField(string sortByField)
        {
            this.fieldToSortBy = sortByField;
        }

        /// <summary>
        /// Sets the sorting order for the drinks. If true, the drinks are sorted in ascending order; otherwise, they are sorted in descending order.
        /// </summary>
        /// <param name="isAscending">Sort order.</param>
        public void SetSortOrder(bool isAscending)
        {
            this.isAscending = isAscending;
        }

        /// <summary>
        /// Sets the category filter for the drinks. This filter is used to display only drinks that belong to the specified categories.
        /// </summary>
        /// <param name="categoryFilter">List of categories to filter the drinks by.</param>
        public void SetCategoryFilter(List<string> categoryFilter)
        {
            this.categoryFilter = categoryFilter;
        }

        /// <summary>
        /// Sets the initial category filter for the drinks. This is used to display only drinks that belong to the specified categories when the page is first loaded.
        /// </summary>
        /// <param name="initialCategoties">List of categories to filter the drinks by.</param>
        public void SetInitialCategoryFilter(List<Category> initialCategoties)
        {
            this.InitialCategories = initialCategoties;
            List<string> categories = new List<string>();
            foreach (Category category in this.InitialCategories)
            {
                categories.Add(category.CategoryName);
            }

            this.SetCategoryFilter(categories);
        }

        /// <summary>
        /// Sets the brand filter for the drinks. This filter is used to display only drinks that belong to the specified brands.
        /// </summary>
        /// <param name="brandFilter">The list of brands to filter the drinks by.</param>
        public void SetBrandFilter(List<string> brandFilter)
        {
            this.brandFilter = brandFilter;
        }

        /// <summary>
        /// Sets the minimum alcohol filter for the drinks. This filter is used to display only drinks that have an alcohol content greater than or equal to the specified value.
        /// </summary>
        /// <param name="minAlcoholFilter">The minimum alcohol content to filter the drinks by.</param>
        public void SetMinAlcoholFilter(float minAlcoholFilter)
        {
            this.minAlcoholFilter = minAlcoholFilter;
        }

        /// <summary>
        /// Sets the maximum alcohol filter for the drinks. This filter is used to display only drinks that have an alcohol content less than or equal to the specified value.
        /// </summary>
        /// <param name="maxAlcoholFilter">The maximum alcohol content to filter the drinks by.</param>
        public void SetMaxAlcoholFilter(float maxAlcoholFilter)
        {
            this.maxAlcoholFilter = maxAlcoholFilter;
        }

        /// <summary>
        /// Sets the minimum rating filter for the drinks. This filter is used to display only drinks that have an average review score greater than or equal to the specified value.
        /// </summary>
        /// <param name="minRatingFilter">The minimum average review score to filter the drinks by.</param>
        public void SetMinRatingFilter(float minRatingFilter)
        {
            this.minRatingFilter = minRatingFilter;
        }

        /// <summary>
        /// Sets the searched terms for the drinks. This filter is used to display only drinks that match the specified search terms.
        /// </summary>
        /// <param name="searchedTerms">The search terms to filter the drinks by.</param>
        public void SetSearchedTerms(string searchedTerms)
        {
            this.searchedTerms = searchedTerms;
        }
    }
}
