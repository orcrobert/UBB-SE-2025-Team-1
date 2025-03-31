using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using WinUIApp.Models;
using WinUIApp.Views.Pages;

namespace WinUIApp.Views.ModelViews
{
    public class SearchPageViewModel(/*ReviewService reviewService*/ Frame frame)
    {
        private readonly Frame _frame = frame;

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
            var beerCategory = new Category(1, "Beer");
            var wineCategory = new Category(2, "Wine");

            var brands = new List<Brand>
            {
                new Brand(1, "Heineken"),
                new Brand(2, "Budweiser"),
                new Brand(3, "Corona"),
                new Brand(4, "Jack Daniels"),
                new Brand(5, "Absolut Vodka")
            };
            return new List<Drink>
            {
                new Drink(1, new List<Category> { beerCategory }, brands[0], 5.0f),
                new Drink(2, new List<Category> { beerCategory }, brands[1], 4.8f),
                new Drink(3, new List<Category> { beerCategory }, brands[2], 6.0f),
                new Drink(4, new List<Category> { wineCategory }, brands[3], 40.0f),
                new Drink(5, new List<Category> { wineCategory }, brands[4], 37.5f)
            };
        }

    }
}

/*

IEnumerable<Drink> DrinksList = new List<Drink>();
DrinksList.Append(new Drink(1, new List<Category> { beerCategory }, brands[0], 5.0f));
DrinksList.Append(new Drink(2, new List<Category> { beerCategory }, brands[1], 4.8f));
DrinksList.Append(new Drink(3, new List<Category> { beerCategory }, brands[2], 6.0f));
DrinksList.Append(new Drink(4, new List<Category> { wineCategory }, brands[3], 40.0f));
DrinksList.Append(new Drink(5, new List<Category> { wineCategory }, brands[4], 37.5f));
 */