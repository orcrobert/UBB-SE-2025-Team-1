using System;
using System.Collections.Generic;
using WinUIApp.Models;


namespace WinUIApp.Services
{
    public class DrinkService
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

        public void addDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent)
        {
            try
            {
                drinkModel.addDrink(drinkName, drinkUrl, categories, brandName, alcoholContent);
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

        public List<Drink> getPersonalDrinkList(int userId, int numberOfDrinks = 1)
        {
            try
            {
                return drinkModel.getPersonalDrinkList(userId, numberOfDrinks);
            }
            catch (Exception e)
            {
                throw new Exception("Error getting personal drink list:", e);
            }
        }

        public bool addToPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                return drinkModel.addToPersonalDrinkList(userId, drinkId);
            }
            catch (Exception e)
            {
                throw new Exception("Error adding drink to personal list:", e);
            }
        }

        public bool deleteFromPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                return drinkModel.deleteFromPersonalDrinkList(userId, drinkId);
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting drink from personal list:", e);
            }
        }
    }
}
