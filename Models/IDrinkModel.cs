using System.Collections.Generic;

namespace WinUIApp.Models
{
    public interface IDrinkModel
    {
        void AddDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent);
        bool AddToPersonalDrinkList(int userId, int drinkId);
        void DeleteDrink(int drinkId);
        bool DeleteFromPersonalDrinkList(int userId, int drinkId);
        int GetCurrentTopVotedDrink();
        List<Brand> GetDrinkBrands();
        Drink? GetDrinkById(int drinkId);
        List<Category> GetDrinkCategories();
        Drink GetDrinkOfTheDay();
        List<Drink> GetDrinks(string? searchTerm, List<string>? brandNameFilters, List<string>? categoryFilters, float? minimumAlcohool, float? maximumAlcohool, Dictionary<string, bool>? orderBy);
        List<Drink> GetPersonalDrinkList(int userId, int numberOfDrinks = 1);
        int GetRandomDrinkId();
        bool IsDrinkInPersonalList(int userId, int drinkId);
        void UpdateDrink(Drink drink);
        void VoteDrinkOfTheDay(int userId, int drinkId);
    }
}