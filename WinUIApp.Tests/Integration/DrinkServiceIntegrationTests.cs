using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Tests.Integration.Stubs;
using Xunit;

namespace WinUIApp.Tests.Integration
{
    public class DrinkServiceIntegrationTests
    {
        private readonly StubDrinkModel _stub;
        private readonly DrinkService _service;

        public DrinkServiceIntegrationTests()
        {
            _stub = new StubDrinkModel();
            _service = new DrinkService(_stub);
        }

        [Fact]
        public void AddDrink_IncreasesDrinkCount()
        {
            int initialCount = _stub.Drinks.Count;

            _service.AddDrink("Cola", "http://example.com/cola.png",
                new List<Category> { new Category(1, "Soft Drink") },
                "TestBrand", 0.0f);

            Assert.Equal(initialCount + 1, _stub.Drinks.Count);
        }

        [Fact]
        public void AddDrink_SetsCorrectDrinkName()
        {
            string expectedName = "Lemonade";

            _service.AddDrink(expectedName, "http://example.com/lemonade.png",
                new List<Category> { new Category(2, "Juice") },
                "FreshBrand", 0.0f);

            var drink = _stub.Drinks.FirstOrDefault(d => d.DrinkName == expectedName);
            Assert.Equal(expectedName, drink?.DrinkName);
        }

        [Fact]
        public void AddDrink_SetsCorrectBrandName()
        {
            string expectedBrand = "QualityBrand";

            _service.AddDrink("Orange Soda", "http://example.com/orange.png",
                new List<Category> { new Category(3, "Soda") },
                expectedBrand, 0.0f);

            var drink = _stub.Drinks.FirstOrDefault(d => d.DrinkName == "Orange Soda");
            Assert.Equal(expectedBrand, drink?.DrinkBrand.BrandName);
        }

        [Fact]
        public void AddDrink_SetsCorrectAlcoholContent()
        {
            float expectedAlcoholContent = 5.0f;
            _service.AddDrink("Beer", "http://example.com/beer.png",
                new List<Category> { new Category(4, "Alcohol") },
                "BreweryBrand", expectedAlcoholContent);
            var drink = _stub.Drinks.FirstOrDefault(d => d.DrinkName == "Beer");
            Assert.Equal(expectedAlcoholContent, drink?.AlcoholContent);
        }

        [Fact]
        public void DeleteDrink_RemovesExistingDrink()
        {
            _service.AddDrink("Water", "http://example.com/water.png",
                new List<Category> { new Category(4, "Pure") },
                "ClearBrand", 0.0f);
            var drink = _stub.Drinks.First();

            _service.DeleteDrink(drink.DrinkId);

            Assert.Null(_stub.Drinks.FirstOrDefault(d => d.DrinkId == drink.DrinkId));
        }

        [Fact]
        public void DeleteDrink_NonexistentDrink_DoesNotChangeCount()
        {
            int initialCount = _stub.Drinks.Count;

            _service.DeleteDrink(999);

            Assert.Equal(initialCount, _stub.Drinks.Count);
        }
    }
}
