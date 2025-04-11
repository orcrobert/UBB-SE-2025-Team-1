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

        public Drink? getDrinkById(int drinkId)
        {
            try
            {
                return drinkModel.GetDrinkById(drinkId);
            }
            catch (Exception e)
            {
                throw new Exception($"Error happened while getting drink with ID {drinkId}:", e);
            }
        }
        public List<Drink> getDrinks(string? searchedTerm, List<string>? brandNameFilter, List<string>? categoryFilter, float? minAlcohol, float? maxAlcohol, Dictionary<string, bool>? orderBy)
        {
            try
            {
                return drinkModel.GetDrinks(searchedTerm, brandNameFilter, categoryFilter, minAlcohol, maxAlcohol, orderBy);

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
                drinkModel.AddDrink(drinkName, drinkUrl, categories, brandName, alcoholContent);
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
                drinkModel.UpdateDrink(drink);
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
                drinkModel.DeleteDrink(drinkId);
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
                return drinkModel.GetDrinkCategories();
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
                return drinkModel.GetDrinkBrands();
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
                return drinkModel.GetPersonalDrinkList(userId, numberOfDrinks);
            }
            catch (Exception e)
            {
                throw new Exception("Error getting personal drink list:", e);
            }
        }

        public bool isDrinkInPersonalList(int userId, int drinkId)
        {
            try
            {
                return drinkModel.IsDrinkInPersonalList(userId, drinkId);
            }
            catch (Exception e)
            {
                throw new Exception("Error adding drink to personal list:", e);
            }
        }

        public bool addToPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                return drinkModel.AddToPersonalDrinkList(userId, drinkId);
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
                return drinkModel.DeleteFromPersonalDrinkList(userId, drinkId);
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting drink from personal list:", e);
            }
        }

        public void voteDrinkOfTheDay(int drinkId, int userId)
        {
            try
            {
                drinkModel.VoteDrinkOfTheDay(drinkId, userId);
            }
            catch (Exception e)
            {
                throw new Exception("Error voting drink:", e);
            }
        }

        public Drink getDrinkOfTheDay()
        {
            try
            {
                return drinkModel.GetDrinkOfTheDay();
            }
            catch (Exception e)
            {
                throw new Exception("Error getting drink of the day:" + e.Message, e);
            }
        }
    }
}
