using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.Views.Components.SearchPageComponents;
using WinUIApp.Views.Pages;

namespace WinUIApp.ViewModels
{
    public class SearchPageViewModel(Frame frame, DrinkService drinkService, ReviewService reviewService)
    {
        private readonly Frame _frame = frame;
        private readonly DrinkService _drinkService = drinkService;

        private ReviewService _reviewService = reviewService;
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

        }

        public void RefreshDrinkList()
        {

        }

        public IEnumerable<DrinkDisplayItem> GetDrinks()
        {
            List<Drink> drinks = _drinkService.getDrinks(null, null, null, null, null, null);
            List<DrinkDisplayItem> displayItems = new List<DrinkDisplayItem>();

            foreach (Drink drink in drinks)
            {
                float averageScore = _reviewService.GetReviewAverageByID(drink.Id);
                DrinkDisplayItem item = new DrinkDisplayItem(drink, averageScore);
                displayItems.Add(item);
            }

            return displayItems;
        }


    }
}
