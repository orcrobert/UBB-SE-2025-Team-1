using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Views.Pages;

namespace WinUIApp.Views.ModelViews
{
    public class SearchPageViewModel(/*ReviewService reviewService*/ Frame frame, DrinkService drinkService)
    {
        private readonly Frame _frame = frame;
        private readonly DrinkService _drinkService = drinkService;

        /*private ReviewService _reviewService = reviewService;
        
         * TO DO
         * add Drink Service field
         * implement methods
        

        public ReviewService ReviewService
        {
            get => _reviewService;
            set => _reviewService = value;
        }
        */

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

        public IEnumerable<Drink> GetDrinks()
        {
            return _drinkService.getDrinks(null, null, null, null, null, null);
        }

    }
}
