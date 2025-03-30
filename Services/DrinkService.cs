using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Models;


namespace WinUIApp.Services
{
    class DrinkService
    {
        public DrinkService() { }

        public List<Drink> getDrinks()
        {
            List<Drink> drinks = [];
            return drinks;
        }

        public void addDrink(List<Category> categories, string brandName, float alcoholContent)
        {
        }

        public void updateDrink(Drink drink)
        {
        }

        public void deleteDrink(int drinkId)
        {
        }
    }
}
