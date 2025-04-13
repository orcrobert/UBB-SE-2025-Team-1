using System.Collections.Generic;
using WinUIApp.Models;

namespace WinUIApp.Services
{
    public interface IDrinkService
    {
        void AddDrink(string inputtedDrinkName, string inputtedDrinkPath, List<Category> inputtedDrinkCategories, string inputtedDrinkBrandName, float inputtedAlcoholPercentage);
        bool AddToUserPersonalDrinkList(int userId, int drinkId);
        void DeleteDrink(int drinkId);
        bool DeleteFromUserPersonalDrinkList(int userId, int drinkId);
        List<Brand> GetDrinkBrandNames();
        Drink? GetDrinkById(int drinkId);
        List<Category> GetDrinkCategories();
        Drink GetDrinkOfTheDay();
        List<Drink> GetDrinks(string? searchKeyword, List<string>? drinkBrandNameFilter, List<string>? drinkCategoryFilter, float? minimumAlcoholPercentage, float? maximumAlcoholPercentage, Dictionary<string, bool>? orderingCriteria);
        List<Drink> GetUserPersonalDrinkList(int userId, int maximumDrinkCount = 1);
        bool IsDrinkInUserPersonalList(int userId, int drinkId);
        void UpdateDrink(Drink drink);
        Drink VoteDrinkOfTheDAy(int userId,int drinkId);
    }
}