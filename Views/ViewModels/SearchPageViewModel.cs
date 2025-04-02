using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.Views.Components.SearchPageComponents;
using WinUIApp.Views.Pages;

namespace WinUIApp.Views.ModelViews
{
    public class SearchPageViewModel(Frame frame, DrinkService drinkService, ReviewService reviewService)
    {
        private readonly Frame _frame = frame;

        private readonly DrinkService _drinkService = drinkService;
        private ReviewService _reviewService = reviewService;

        private bool _isAscending = true;
        private string _sortByField = "Name";

        private List<string>? _categoryFilter;
        private List<string>? _brandFilter;

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

        /*
         * TO DO
         * implement methods
        */

        public ReviewService ReviewService
        {
            get => _reviewService;
            set => _reviewService = value;
        }

        public void OpenDrinkDetailPage(int id)
        {
            _frame.Navigate(typeof(DrinkDetailPage), id);
        }

        public void ClearFilters()
        {
            _categoryFilter = null;
            _brandFilter = null;
            //add all filters
        }

        public void RefreshDrinkList()
        {
            //
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
                    searchedTerm: null,
                    brandNameFilter: _brandFilter,
                    categoryFilter: _categoryFilter,
                    minAlcohol: null,
                    maxAlcohol: null,
                    orderBy: orderBy
                );

                displayItems = new List<DrinkDisplayItem>();
                foreach (Drink drink in drinks)
                {
                    float averageScore = _reviewService.GetReviewAverageByID(drink.Id);
                    displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                }

            }
            else
            {
                List<Drink> drinks = _drinkService.getDrinks(
                    searchedTerm: null,
                    brandNameFilter: _brandFilter,
                    categoryFilter: _categoryFilter,
                    minAlcohol: null,
                    maxAlcohol: null,
                    orderBy: null
                );

                displayItems = new List<DrinkDisplayItem>();
                foreach (Drink drink in drinks)
                {
                    float averageScore = _reviewService.GetReviewAverageByID(drink.Id);
                    displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                }

                displayItems = _isAscending
                    ? displayItems.OrderBy(item => item.AverageReviewScore).ToList()
                    : displayItems.OrderByDescending(item => item.AverageReviewScore).ToList();

            }/*
            if (_brandFilter != null)
            {
                if (_brandFilter.Count >= 1)
                {
                    Debug.WriteLine(_brandFilter[0]);
                }
            }
            Debug.WriteLine("DrinkList:");
            Debug.WriteLine("len" + displayItems.Count);
            Debug.WriteLine("xxx");*/

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

        public void SetBrandFilter(List<string> brandFilter)
        {
            _brandFilter = brandFilter;
        }

    }
}
