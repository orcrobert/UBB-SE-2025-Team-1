using Mysqlx.Crud;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServices;
using WinUIApp.Views;
using WinUIApp.Views.Components.SearchPageComponents;
using WinUIApp.Views.Pages;

namespace WinUIApp.ViewModels
{
    /// <summary>
    /// ViewModel for the SearchPage. Manages the display of drinks based on various filters and sorting options.
    /// </summary>
    /// <param name="drinkService">The drink service used to manage drinks.</param>
    /// <param name="reviewService">The review service used to manage reviews.</param>
    public class SearchPageViewModel(IDrinkService drinkService, IDrinkReviewService reviewService)
    {
        private const string nameField = "Name";

        private readonly IDrinkService _drinkService = drinkService;
        private IDrinkReviewService _reviewService = reviewService;

        private bool _isAscending = true;
        private string _fieldToSortBy = nameField;

        private List<string>? _categoryFilter;
        private List<string>? _brandFilter;
        private float? _minAlcoholFilter;
        private float? _maxAlcoholFilter;
        private float? _minRatingFilter;
        private string? _searchedTerms;

        /// <summary>
        /// The list of categories that are initially displayed on the search page.
        /// </summary>
        public List<Category> InitialCategories { get; set; }

        /// <summary>
        /// Indicates whether the sorting order of the drinks is ascending or descending.
        /// </summary>
        public bool IsAscending
        {
            get => _isAscending;
            set => _isAscending = value;
        }

        /// <summary>
        /// Indicates the field by which the drinks are sorted. Default is "Name".
        /// </summary>
        public string FieldToSortBy
        {
            get => _fieldToSortBy;
            set => _fieldToSortBy = value;
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
            _categoryFilter = null;
            _brandFilter = null;
            _minAlcoholFilter = null;
            _maxAlcoholFilter = null;
            _minRatingFilter = null;
            _searchedTerms = null;
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

            if (_fieldToSortBy == nameField || _fieldToSortBy == "Alcohol Content")
            {
                string sortField;
                if (_fieldToSortBy == nameField)
                {
                    sortField = "D.DrinkName";
                }
                else
                {
                    sortField = "D.AlcoholContent";
                }

                var orderBy = new Dictionary<string, bool>
                {
                    { sortField, _isAscending }
                };
                List<Drink> drinks = _drinkService.GetDrinks(
                    searchKeyword: _searchedTerms,
                    drinkBrandNameFilter: _brandFilter,
                    drinkCategoryFilter: _categoryFilter,
                    minimumAlcoholPercentage: _minAlcoholFilter,
                    maximumAlcoholPercentage: _maxAlcoholFilter,
                    orderingCriteria: orderBy
                );

                displayItems = new List<DrinkDisplayItem>();
                foreach (Drink drink in drinks)
                {
                    float averageScore = _reviewService.GetReviewAverageByID(drink.DrinkId);
                    if (_minRatingFilter == null)
                    {
                        displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                    }
                    else
                    {
                        if (averageScore >= _minRatingFilter)
                        {
                            displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                        }
                    }
                }

            }
            else
            {

                List<Drink> drinks = _drinkService.GetDrinks(
                    searchKeyword: _searchedTerms,
                    drinkBrandNameFilter: _brandFilter,
                    drinkCategoryFilter: _categoryFilter,
                    minimumAlcoholPercentage: _minAlcoholFilter,
                    maximumAlcoholPercentage: _maxAlcoholFilter,
                    orderingCriteria: null
                );

                displayItems = new List<DrinkDisplayItem>();
                foreach (Drink drink in drinks)
                {
                    float averageScore = _reviewService.GetReviewAverageByID(drink.DrinkId);
                    if (_minRatingFilter == null)
                    {
                        displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                    }
                    else
                    {
                        if (averageScore >= _minRatingFilter)
                        {
                            displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                        }
                    }
                }

                if (_isAscending)
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
            return _drinkService.GetDrinkCategories();
        }

        /// <summary>
        /// Retrieves the list of drink brands from the drink service.
        /// </summary>
        /// <returns>The list of drink brands.</returns>
        public IEnumerable<Brand> GetBrands()
        {
            return _drinkService.GetDrinkBrandNames();
        }

        /// <summary>
        /// Sets the sorting field for the drinks. The default is "Name".
        /// </summary>
        /// <param name="sortByField"></param>
        public void SetSortByField(string sortByField)
        {
            _fieldToSortBy = sortByField;
        }

        /// <summary>
        /// Sets the sorting order for the drinks. If true, the drinks are sorted in ascending order; otherwise, they are sorted in descending order.
        /// </summary>
        /// <param name="isAscending"></param>
        public void SetSortOrder(bool isAscending)
        {
            _isAscending = isAscending;
        }

        /// <summary>
        /// Sets the category filter for the drinks. This filter is used to display only drinks that belong to the specified categories.
        /// </summary>
        /// <param name="categoryFilter">List of categories to filter the drinks by.</param>
        public void SetCategoryFilter(List<string> categoryFilter)
        {
            _categoryFilter = categoryFilter;
        }

        /// <summary>
        /// Sets the initial category filter for the drinks. This is used to display only drinks that belong to the specified categories when the page is first loaded.
        /// </summary>
        /// <param name="initialCategoties">List of categories to filter the drinks by.</param>
        public void SetInitialCategoryFilter(List<Category> initialCategoties)
        {
            InitialCategories = initialCategoties;
            List<string> categories = new List<string>();
            foreach (Category category in InitialCategories)
            {
                categories.Add(category.CategoryName);
            }
            SetCategoryFilter(categories);
        }

        /// <summary>
        /// Sets the brand filter for the drinks. This filter is used to display only drinks that belong to the specified brands.
        /// </summary>
        /// <param name="brandFilter">The list of brands to filter the drinks by.</param>
        public void SetBrandFilter(List<string> brandFilter)
        {
            _brandFilter = brandFilter;
        }

        /// <summary>
        /// Sets the minimum alcohol filter for the drinks. This filter is used to display only drinks that have an alcohol content greater than or equal to the specified value.
        /// </summary>
        /// <param name="minAlcoholFilter">The minimum alcohol content to filter the drinks by.</param>
        public void SetMinAlcoholFilter(float minAlcoholFilter)
        {
            _minAlcoholFilter = minAlcoholFilter;
        }

        /// <summary>
        /// Sets the maximum alcohol filter for the drinks. This filter is used to display only drinks that have an alcohol content less than or equal to the specified value.
        /// </summary>
        /// <param name="maxAlcoholFilter">The maximum alcohol content to filter the drinks by.</param>
        public void SetMaxAlcoholFilter(float maxAlcoholFilter)
        {
            _maxAlcoholFilter = maxAlcoholFilter;
        }

        /// <summary>
        /// Sets the minimum rating filter for the drinks. This filter is used to display only drinks that have an average review score greater than or equal to the specified value.
        /// </summary>
        /// <param name="minRatingFilter">The minimum average review score to filter the drinks by.</param>
        public void SetMinRatingFilter(float minRatingFilter)
        {
            _minRatingFilter = minRatingFilter;
        }

        /// <summary>
        /// Sets the searched terms for the drinks. This filter is used to display only drinks that match the specified search terms.
        /// </summary>
        /// <param name="searchedTerms">The search terms to filter the drinks by.</param>
        public void SetSearchedTerms(string searchedTerms)
        {
            _searchedTerms = searchedTerms;
        }

    }
}
