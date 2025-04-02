using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.Views.Components.SearchPageComponents;
using WinUIApp.Views.Pages;

namespace WinUIApp.Views.ModelViews
{
    public class SearchPageViewModel(DrinkService drinkService, ReviewService reviewService)
    {

        private readonly DrinkService _drinkService = drinkService;
        private ReviewService _reviewService = reviewService;

        private bool _isAscending = true;
        private string _sortByField = "Name";

        private List<string>? _categoryFilter;
        private List<string>? _brandFilter;
        private float? _minAlcoholFilter;
        private float? _maxAlcoholFilter;
        private float? _minRating;
        private string? _searchedTerms;

        public List<Category> InitialCategories { get; set; }


        public bool IsAscending
        {
            get => _isAscending;
            set => _isAscending = value;
        }

        public string SortByField
        {
            get => _sortByField;
            set => _sortByField = value;
        }

        public ReviewService ReviewService
        {
            get => _reviewService;
            set => _reviewService = value;
        }

        public void OpenDrinkDetailPage(int id)
        {
            MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), id);
        }

        public void ClearFilters()
        {
            _categoryFilter = null;
            _brandFilter = null;
            _minAlcoholFilter = null;
            _maxAlcoholFilter = null;
            _minRating = null;
            _searchedTerms = null;
        }


        public IEnumerable<DrinkDisplayItem> GetDrinks()
        {
            List<DrinkDisplayItem> displayItems = new List<DrinkDisplayItem>();

            if (_sortByField == "Name" || _sortByField == "Alcohol Content")
            {
                var orderBy = new Dictionary<string, bool>
                {
                    { _sortByField == "Name" ? "D.DrinkName" : "D.AlcoholContent", _isAscending }
                };

                List<Drink> drinks = _drinkService.getDrinks(
                    searchedTerm: _searchedTerms,
                    brandNameFilter: _brandFilter,
                    categoryFilter: _categoryFilter,
                    minAlcohol: _minAlcoholFilter,
                    maxAlcohol: _maxAlcoholFilter,
                    orderBy: orderBy
                );

                displayItems = new List<DrinkDisplayItem>();
                foreach (Drink drink in drinks)
                {
                    float averageScore = _reviewService.GetReviewAverageByID(drink.Id);
                    if (_minRating == null)
                    {
                        displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                    }
                    else
                    {
                        if (averageScore >= _minRating)
                        {
                            displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                        }
                    }
                }

            }
            else
            {
                List<Drink> drinks = _drinkService.getDrinks(
                    searchedTerm: _searchedTerms,
                    brandNameFilter: _brandFilter,
                    categoryFilter: _categoryFilter,
                    minAlcohol: _minAlcoholFilter,
                    maxAlcohol: _maxAlcoholFilter,
                    orderBy: null
                );

                displayItems = new List<DrinkDisplayItem>();
                foreach (Drink drink in drinks)
                {
                    float averageScore = _reviewService.GetReviewAverageByID(drink.Id);
                    if (_minRating == null)
                    {
                        displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                    }
                    else
                    {
                        if (averageScore >= _minRating)
                        {
                            displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                        }
                    }
                }

                displayItems = _isAscending
                    ? displayItems.OrderBy(item => item.AverageReviewScore).ToList()
                    : displayItems.OrderByDescending(item => item.AverageReviewScore).ToList();

            }

            return displayItems;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _drinkService.getDrinkCategories();
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _drinkService.getDrinkBrands();
        }

        public void SetSortByField(string sortByField)
        {
            _sortByField = sortByField;
        }

        public void SetSortOrder(bool isAscending)
        {
            _isAscending = isAscending;
        }

        public void SetCategoryFilter(List<string> categoryFilter)
        {
            _categoryFilter = categoryFilter;
        }

        public void SetInitialCategoryFilter(List<Category> initialCategoties)
        {
            InitialCategories = initialCategoties;
            List<string> categories = new List<string>();
            foreach (Category category in InitialCategories)
            {
                categories.Add(category.Name);
            }
            SetCategoryFilter(categories);
        }

        public void SetBrandFilter(List<string> brandFilter)
        {
            _brandFilter = brandFilter;
        }

        public void SetMinAlcoholFilter(float minAlcoholFilter)
        {
            _minAlcoholFilter = minAlcoholFilter;
        }

        public void SetMaxAlcoholFilter(float maxAlcoholFilter)
        {
            _maxAlcoholFilter = maxAlcoholFilter;
        }

        public void SetMinRatingFilter(float minRatingFilter)
        {
            _minRating = minRatingFilter;
        }

        public void SetSearchedTerms(string searchedTerms)
        {
            _searchedTerms = searchedTerms;
        }

    }
}
