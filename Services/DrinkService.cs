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
        private DrinkModel drinkModel;
        public DrinkService()
        {
            drinkModel = new DrinkModel();
        }

        public List<Drink> getDrinks(string? searchedTerm, List<string>? brandNameFilter, List<string>? categoryFilter, float? minAlcohol, float? maxAlcohol, Dictionary<string, bool>? orderBy)
        {
            try
            {
                return drinkModel.getDrinks(searchedTerm, brandNameFilter, categoryFilter, minAlcohol, maxAlcohol, orderBy);

            }
            catch (Exception e)
            {
                throw new Exception("Error happened while getting drinks:", e);
            }
        }

        public void addDrink(List<Category> categories, string brandName, float alcoholContent)
        {
            try
            {
                drinkModel.addDrink(categories, brandName, alcoholContent);
            }
            catch (Exception e)
            {
                throw new Exception("Error happened while adding a drink:", e);
            }
        }

        public void updateDrink(Drink drink)
        {
            try
            {
                drinkModel.updateDrink(drink);
            }
            catch (Exception e)
            {
                throw new Exception("Error happened while updating a drink:", e);
            }
        }

        public void deleteDrink(int drinkId)
        {
            try
            {
                drinkModel.deleteDrink(drinkId);
            }
            catch (Exception e)
            {
                throw new Exception("Error happened while deleting a drink:", e);
            }
        }

        public List<Category> getDrinkCategories()
        {
            try
            {
                return drinkModel.getDrinkCategories();
            }
            catch (Exception e)
            {
                throw new Exception("Error happened while getting drink categories:", e);
            }
        }

        public List<Brand> getDrinkBrands()
        {
            try
            {
                return drinkModel.getDrinkBrands();
            }
            catch (Exception e)
            {
                throw new Exception("Error happened while getting drink brands:", e);
            }
        }
    }
}
