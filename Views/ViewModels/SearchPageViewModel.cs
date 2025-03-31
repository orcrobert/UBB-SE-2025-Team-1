using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Diagnostics;
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

        public bool IsAscending
        {
            get { return _isAscending; }
            set { _isAscending = value; }
        }
        /*
         * TO DO
         * add Drink Service field
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
            //
        }

        public void RefreshDrinkList()
        {

        }

        public IEnumerable<DrinkDisplayItem> GetDrinks()
        {
            var orderBy = new Dictionary<string, bool>
            {
                { "D.DrinkName", _isAscending } // true = ASC, false = DESC
            };

            List<Drink> drinks = _drinkService.getDrinks(
                searchedTerm: null,        // Add search term if needed
                brandNameFilter: null,     // Add filters if needed
                categoryFilter: null,      // Add filters if needed
                minAlcohol: null,          // Add min alcohol if needed
                maxAlcohol: null,          // Add max alcohol if needed
                orderBy: orderBy           // Pass sorting parameter
            );
            List<DrinkDisplayItem> displayItems = new List<DrinkDisplayItem>();

            foreach (Drink drink in drinks)
            {
                float averageScore = _reviewService.GetReviewAverageByID(drink.Id);
                DrinkDisplayItem item = new DrinkDisplayItem(drink, averageScore);
                displayItems.Add(item);
            }
            Debug.WriteLine($"Fetched {displayItems.Count} drinks, sorted {(_isAscending ? "ascending" : "descending")} by name");

            return displayItems;
        }

        public void SetSortOrder(bool isAscending)
        {
            _isAscending = isAscending;
            Debug.WriteLine($"Sort order set to: {(_isAscending ? "Ascending" : "Descending")}");
        }

    }
}
