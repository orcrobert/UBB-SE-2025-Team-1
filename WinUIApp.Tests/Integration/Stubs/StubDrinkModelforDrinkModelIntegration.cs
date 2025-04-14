using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

namespace WinUIApp.Tests.Integration.Stubs
{
    public class StubDrinkModelforDrinkModelIntegration : IDrinkModel
    {
        private readonly List<Drink> _drinks = new();
        private readonly List<Brand> _brands = new();
        private readonly List<Category> _categories = new();
        private readonly Dictionary<int, List<int>> _userDrinkLists = new();
        private readonly Dictionary<int, int> _votes = new();
        private int _drinkOfTheDayId = -1;

        public StubDrinkModelforDrinkModelIntegration()
        {
            // Default constructor for the stub
        }

        public void AddDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent)
        {
            var brand = _brands.FirstOrDefault(b => b.BrandName == brandName) ?? new Brand(_brands.Count + 1, brandName);
            if (!_brands.Contains(brand)) _brands.Add(brand);

            var drink = new Drink(_drinks.Count + 1, drinkName, drinkUrl, categories, brand, alcoholContent);
            _drinks.Add(drink);
        }

        public void DeleteDrink(int drinkId)
        {
            _drinks.RemoveAll(d => d.DrinkId == drinkId);
        }

        public Drink? GetDrinkById(int drinkId)
        {
            return _drinks.FirstOrDefault(d => d.DrinkId == drinkId);
        }

        public List<Drink> GetDrinks(string? searchTerm, List<string>? brandNameFilters, List<string>? categoryFilters, float? minimumAlcohol, float? maximumAlcohol, Dictionary<string, bool>? orderBy)
        {
            var query = _drinks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(d => d.DrinkName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (brandNameFilters != null && brandNameFilters.Any())
            {
                query = query.Where(d => brandNameFilters.Contains(d.DrinkBrand.BrandName));
            }

            if (categoryFilters != null && categoryFilters.Any())
            {
                query = query.Where(d => d.CategoryList.Any(c => categoryFilters.Contains(c.CategoryName)));
            }

            if (minimumAlcohol.HasValue)
            {
                query = query.Where(d => d.AlcoholContent >= minimumAlcohol.Value);
            }

            if (maximumAlcohol.HasValue)
            {
                query = query.Where(d => d.AlcoholContent <= maximumAlcohol.Value);
            }

            return query.ToList();
        }

        public void UpdateDrink(Drink drink)
        {
            var existingDrink = _drinks.FirstOrDefault(d => d.DrinkId == drink.DrinkId);
            if (existingDrink != null)
            {
                existingDrink.DrinkName = drink.DrinkName;
                existingDrink.DrinkImageUrl = drink.DrinkImageUrl;
                existingDrink.AlcoholContent = drink.AlcoholContent;
                existingDrink.CategoryList = drink.CategoryList;
                existingDrink.DrinkBrand = drink.DrinkBrand;
            }
        }

        public List<Category> GetDrinkCategories()
        {
            return _categories;
        }

        public List<Brand> GetDrinkBrands()
        {
            return _brands;
        }

        public bool AddToPersonalDrinkList(int userId, int drinkId)
        {
            if (!_userDrinkLists.ContainsKey(userId))
            {
                _userDrinkLists[userId] = new List<int>();
            }

            if (!_userDrinkLists[userId].Contains(drinkId))
            {
                _userDrinkLists[userId].Add(drinkId);
                return true;
            }

            return false;
        }

        public bool DeleteFromPersonalDrinkList(int userId, int drinkId)
        {
            if (_userDrinkLists.ContainsKey(userId))
            {
                return _userDrinkLists[userId].Remove(drinkId);
            }

            return false;
        }

        public bool IsDrinkInPersonalList(int userId, int drinkId)
        {
            return _userDrinkLists.ContainsKey(userId) && _userDrinkLists[userId].Contains(drinkId);
        }

        public List<Drink> GetPersonalDrinkList(int userId, int numberOfDrinks = 1)
        {
            if (_userDrinkLists.ContainsKey(userId))
            {
                var drinkIds = _userDrinkLists[userId].Take(numberOfDrinks).ToList();
                return _drinks.Where(d => drinkIds.Contains(d.DrinkId)).ToList();
            }

            return new List<Drink>();
        }

        public void VoteDrinkOfTheDay(int userId, int drinkId)
        {
            _votes[userId] = drinkId;
        }

        public Drink GetDrinkOfTheDay()
        {
            if (_drinkOfTheDayId == -1)
            {
                _drinkOfTheDayId = _votes.GroupBy(v => v.Value)
                                         .OrderByDescending(g => g.Count())
                                         .Select(g => g.Key)
                                         .FirstOrDefault();
            }

            return GetDrinkById(_drinkOfTheDayId) ?? throw new InvalidOperationException("No Drink of the Day set.");
        }

        public void SetDrinkOfTheDay(int drinkId)
        {
            _drinkOfTheDayId = drinkId;
        }

        public int GetCurrentTopVotedDrink()
        {
            return _votes.GroupBy(v => v.Value)
                         .OrderByDescending(g => g.Count())
                         .Select(g => g.Key)
                         .FirstOrDefault();
        }

        public int GetRandomDrinkId()
        {
            return _drinks.OrderBy(_ => Guid.NewGuid()).FirstOrDefault()?.DrinkId ?? -1;
        }
    }
}
