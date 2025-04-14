using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;

namespace WinUIApp.Tests.Integration.Stubs
{
    // A stub implementation of IDrinkModel for integration testing.
    public class StubDrinkModel : IDrinkModel
    {
        public List<Drink> Drinks { get; private set; } = new List<Drink>();

        public List<(int userId, int drinkId)> PersonalList { get; private set; } = new List<(int userId, int drinkId)>();

        public void AddDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent)
        {
            int newId = Drinks.Count + 1;
            var brand = new Brand(newId, brandName);
            var drink = new Drink(newId, drinkName, drinkUrl, categories, brand, alcoholContent);
            Drinks.Add(drink);
        }

        public void DeleteDrink(int drinkId)
        {
            Drinks.RemoveAll(d => d.DrinkId == drinkId);
        }

        public Drink? GetDrinkById(int drinkId) =>
            Drinks.FirstOrDefault(d => d.DrinkId == drinkId);

        public List<Drink> GetDrinks(string? searchTerm, List<string>? drinkBrandNameFilter, List<string>? drinkCategoryFilter, float? minimumAlcoholPercentage, float? maximumAlcoholPercentage, Dictionary<string, bool>? orderingCriteria) =>
            Drinks;

        public void UpdateDrink(Drink drink)
        {
            int index = Drinks.FindIndex(d => d.DrinkId == drink.DrinkId);
            if (index != -1) Drinks[index] = drink;
        }

        public List<Category> GetDrinkCategories() => new List<Category>();

        public List<Brand> GetDrinkBrands() => new List<Brand>();

        public bool AddToPersonalDrinkList(int userId, int drinkId)
        {
            if (!PersonalList.Any(item => item.userId == userId && item.drinkId == drinkId))
            {
                PersonalList.Add((userId, drinkId));
                return true;
            }
            return false;
        }

        public bool DeleteFromPersonalDrinkList(int userId, int drinkId)
        {
            int before = PersonalList.Count;
            PersonalList.RemoveAll(item => item.userId == userId && item.drinkId == drinkId);
            return PersonalList.Count < before;
        }

        public bool IsDrinkInPersonalList(int userId, int drinkId) =>
            PersonalList.Any(item => item.userId == userId && item.drinkId == drinkId);

        public List<Drink> GetPersonalDrinkList(int userId, int numberOfDrinks = 1)
        {
            var ids = PersonalList.Where(item => item.userId == userId).Select(item => item.drinkId);
            return Drinks.Where(d => ids.Contains(d.DrinkId)).Take(numberOfDrinks).ToList();
        }

        int IDrinkModel.GetCurrentTopVotedDrink()
        {
            throw new NotImplementedException();
        }

        Drink IDrinkModel.GetDrinkOfTheDay()
        {
            throw new NotImplementedException();
        }

        int IDrinkModel.GetRandomDrinkId()
        {
            throw new NotImplementedException();
        }

        void IDrinkModel.VoteDrinkOfTheDay(int userId, int drinkId)
        {
            throw new NotImplementedException();
        }
    }
}
