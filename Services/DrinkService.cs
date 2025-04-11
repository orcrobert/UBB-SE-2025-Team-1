using System;
using System.Collections.Generic;
using WinUIApp.Models;


namespace WinUIApp.Services
{
    public class DrinkService
    {
        private DrinkModel _drinkModel;
        private const int DefaultPersonalDrinkCount = 1;
        public DrinkService()
        {
            _drinkModel = new DrinkModel();
        }

        public Drink? GetDrinkById(int drinkId)
        {
            try
            {
                return _drinkModel.getDrinkById(drinkId);
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
                return _drinkModel.getDrinks(searchKeyword, drinkBrandNameFilter, drinkCategoryFilter, minimumAlcoholPercentage, maximumAlcoholPercentage, orderingCriteria);

            }
            catch (Exception drinksRetrievalException)
            {
                throw new Exception("Error happened while getting drinks:", drinksRetrievalException);
            }
        }

        public void AddDrink(string inputedDrinkName, string inputedDrinkPath, List<Category> inputedDrinkCategories, string inputedDrinkBrandName, float inputedAlcoholPercentage)
        {
            try
            {
                _drinkModel.addDrink(inputedDrinkName, inputedDrinkPath, inputedDrinkCategories, inputedDrinkBrandName, inputedAlcoholPercentage);
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
                _drinkModel.updateDrink(drink);
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
                _drinkModel.deleteDrink(drinkId);
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
                return _drinkModel.getDrinkCategories();
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
                return _drinkModel.getDrinkBrands();
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
                return _drinkModel.getPersonalDrinkList(userId, maximumDrinkCount);
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
                return _drinkModel.isDrinkInPersonalList(userId, drinkId);
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
                return _drinkModel.addToPersonalDrinkList(userId, drinkId);
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
                return _drinkModel.deleteFromPersonalDrinkList(userId, drinkId);
            }
            catch (Exception deleteFromUserPersonalDrinkListException)
            {
                throw new Exception("Error deleting drink from personal list:", deleteFromUserPersonalDrinkListException);
            }
        }

        public void VoteDrinkOfTheDay(int drinkId, int userId)
        {
            try
            {
                _drinkModel.voteDrinkOfTheDay(drinkId, userId);
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
                return _drinkModel.getDrinkOfTheDay();
            }
            catch (Exception getDrinkOfTheDayException)
            {
                throw new Exception("Error getting drink of the day:", getDrinkOfTheDayException);
            }
        }
    }
}
