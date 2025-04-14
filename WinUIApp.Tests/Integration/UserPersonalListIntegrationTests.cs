using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Tests.Integration.Stubs;
using Xunit;

namespace WinUIApp.Tests.Integration
{
    public class UserPersonalListIntegrationTests
    {
        private readonly StubDrinkModel _stub;
        private readonly DrinkService _service;
        private readonly int _userId = 1;

        public UserPersonalListIntegrationTests()
        {
            _stub = new StubDrinkModel();
            _service = new DrinkService(_stub);
        }

        [Fact]
        public void AddToPersonalList_IncreasesPersonalListCount()
        {
            _service.AddDrink("Smoothie", "http://example.com/smoothie.png",
                new List<Category> { new Category(5, "Healthy") },
                "FruitBrand", 0.0f);

            var drink = _stub.Drinks.FirstOrDefault(d => d.DrinkName == "Smoothie");

            bool result = _service.AddToUserPersonalDrinkList(_userId, drink.DrinkId);

            Assert.True(result);
        }

        [Fact]
        public void AddToPersonalList_DuplicateReturnsFalse()
        {
            _service.AddDrink("Iced Tea", "http://example.com/icedtea.png",
                new List<Category> { new Category(6, "Tea") },
                "CoolBrand", 0.0f);

            var drink = _stub.Drinks.FirstOrDefault(d => d.DrinkName == "Iced Tea");

            bool firstAdd = _service.AddToUserPersonalDrinkList(_userId, drink.DrinkId);

            bool secondAdd = _service.AddToUserPersonalDrinkList(_userId, drink.DrinkId);

            Assert.False(secondAdd);
        }

        [Fact]
        public void DeleteFromPersonalList_RemovesExistingEntry()
        {
            _service.AddDrink("Limeade", "http://example.com/limeade.png",
                new List<Category> { new Category(7, "Citrus") },
                "FreshBrand", 0.0f);

            var drink = _stub.Drinks.FirstOrDefault(d => d.DrinkName == "Limeade");
            _service.AddToUserPersonalDrinkList(_userId, drink.DrinkId);

            bool result = _service.DeleteFromUserPersonalDrinkList(_userId, drink.DrinkId);

            Assert.False(_stub.IsDrinkInPersonalList(_userId, drink.DrinkId));
        }

        [Fact]
        public void DeleteFromPersonalList_NonexistentEntry_ReturnsFalse()
        {
            int nonExistentDrinkId = 999;

            bool result = _service.DeleteFromUserPersonalDrinkList(_userId, nonExistentDrinkId);

            Assert.False(result);
        }
    }
}
