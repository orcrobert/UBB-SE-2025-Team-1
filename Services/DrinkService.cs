using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;


namespace WinUIApp.Services
{
    public class DrinkService : IDrinkService
    {
        private readonly DrinkModel drinkModel;
        private const int DefaultPersonalDrinkCount = 1;
        public DrinkService()
        {
            drinkModel = new DrinkModel();
        }

        public Drink? GetDrinkById(int drinkId)
        {
            try
            {
                return drinkModel.GetDrinkById(drinkId);
            }
            catch (Exception drinkRetrievalException)
            {
                throw new Exception($"Error happened while getting drink with ID {drinkId}:", drinkRetrievalException);
            }
        }
        public List<Drink> GetDrinks(string? searchKeyword, List<string>? drinkBrandNameFilter, List<string>? drinkCategoryFilter, float? minimumAlcoholPercentage, float? maximumAlcoholPercentage, Dictionary<string, bool>? orderingCriteria)
        {
            try
            {
                return drinkModel.GetDrinks(searchKeyword, drinkBrandNameFilter, drinkCategoryFilter, minimumAlcoholPercentage, maximumAlcoholPercentage, orderingCriteria);
            }
            catch (Exception drinksRetrievalException)
            {
                throw new Exception("Error happened while getting drinks:", drinksRetrievalException);
            }
        }

        public void AddDrink(string inputtedDrinkName, string inputtedDrinkPath, List<Category> inputtedDrinkCategories, string inputtedDrinkBrandName, float inputtedAlcoholPercentage)
        {
            try
            {
                drinkModel.AddDrink(inputtedDrinkName, inputtedDrinkPath, inputtedDrinkCategories, inputtedDrinkBrandName, inputtedAlcoholPercentage);
            }
            catch (Exception addingDrinkException)
            {
                throw new Exception("Error happened while adding a drink:", addingDrinkException);
            }
        }

        public void UpdateDrink(Drink drink)
        {
            try
            {
                drinkModel.UpdateDrink(drink);
            }
            catch (Exception updateDrinkException)
            {
                throw new Exception("Error happened while updating a drink:", updateDrinkException);
            }
        }

        public void DeleteDrink(int drinkId)
        {
            try
            {
                drinkModel.DeleteDrink(drinkId);
            }
            catch (Exception deleteDrinkException)
            {
                throw new Exception("Error happened while deleting a drink:", deleteDrinkException);
            }
        }

        public List<Category> GetDrinkCategories()
        {
            try
            {
                return drinkModel.GetDrinkCategories();
            }
            catch (Exception drinkCategoriesRetrievalException)
            {
                throw new Exception("Error happened while getting drink categories:", drinkCategoriesRetrievalException);
            }
        }

        public List<Brand> GetDrinkBrandNames()
        {
            try
            {
                return drinkModel.GetDrinkBrands();
            }
            catch (Exception drinkBrandNamesRetrievalException)
            {
                throw new Exception("Error happened while getting drink brands:", drinkBrandNamesRetrievalException);
            }
        }
        public List<Drink> GetUserPersonalDrinkList(int userId, int maximumDrinkCount = DefaultPersonalDrinkCount)
        {
            try
            {
                return drinkModel.GetPersonalDrinkList(userId, maximumDrinkCount);
            }
            catch (Exception personalDrinkListRetrievalException)
            {
                throw new Exception("Error getting personal drink list:", personalDrinkListRetrievalException);
            }
        }

        public bool IsDrinkInUserPersonalList(int userId, int drinkId)
        {
            try
            {
                return drinkModel.IsDrinkInPersonalList(userId, drinkId);
            }
            catch (Exception checkingUserPersonalListException)
            {
                throw new Exception("Error checking if the drink is in the user's personal list.", checkingUserPersonalListException);
            }
        }

        public bool AddToUserPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                return drinkModel.AddToPersonalDrinkList(userId, drinkId);
            }
            catch (Exception addDrinkToUserPersonalListException)
            {
                throw new Exception("Error adding drink to personal list:", addDrinkToUserPersonalListException);
            }
        }

        public bool DeleteFromUserPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                return drinkModel.DeleteFromPersonalDrinkList(userId, drinkId);
            }
            catch (Exception deleteFromUserPersonalDrinkListException)
            {
                throw new Exception("Error deleting drink from personal list:", deleteFromUserPersonalDrinkListException);
            }
        }

        public Drink VoteDrinkOfTheDAy(int drinkId, int userId)
        {
            try
            {
                drinkModel.VoteDrinkOfTheDay(drinkId, userId);
                return drinkModel.GetDrinkById(drinkId) ?? throw new Exception("Drink not found after voting.");
            }
            catch (Exception voteDrinkOfTheDayException)
            {
                throw new Exception("Error voting drink:", voteDrinkOfTheDayException);
            }
        }

        public Drink GetDrinkOfTheDay()
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
