using System;
using System.Collections.Generic;
using Moq;
using WinUIApp.Models;
using WinUIApp.Services;
using Xunit;

namespace WinUIApp.Tests.Services.DrinkServiceTests
{
    public class DrinkServiceTests
    {
        private DrinkService _drinkService;
        private Mock<IDrinkModel> _mockModel;

        private List<Category> _testCategories;
        private Brand _testBrand;
        private Drink _testDrink;

        public DrinkServiceTests()
        {
            _mockModel = new Mock<IDrinkModel>();
            _drinkService = new DrinkService(_mockModel.Object);

            _testCategories = new List<Category> { new Category(1,"Juice") };
            _testBrand = new Brand(1, "FreshCo");
            _testDrink = new Drink(1, "Orange Juice", "orange.jpg", _testCategories, _testBrand, 0);
        }

        [Fact]
        public void GetDrinkById_ExistingId_ReturnsCorrectDrink()
        {
            _mockModel.Setup(m => m.GetDrinkById(1)).Returns(_testDrink);

            var result = _drinkService.GetDrinkById(1);

            Assert.NotNull(result);
            Assert.Equal("Orange Juice", result.DrinkName);
            Assert.Equal(0, result.AlcoholContent);
        }

        [Fact]
        public void GetDrinkById_InvalidId_ReturnsNull()
        {
            _mockModel.Setup(m => m.GetDrinkById(It.IsAny<int>())).Returns((Drink)null);

            var result = _drinkService.GetDrinkById(999);

            Assert.Null(result);
        }

        [Fact]
        public void GetDrinks_FiltersCorrectly_ByBrandAndAlcohol()
        {
            var drinks = new List<Drink>
            {
                new Drink(1, "Vodka", "vodka.png", new List<Category>(), new Brand(1, "Ice"), 40),
                new Drink(2, "Juice", "juice.png", new List<Category>(), new Brand(2, "FruitLand"), 0),
                new Drink(3, "Beer", "beer.png", new List<Category>(), new Brand(1, "Ice"), 5)
            };

            _mockModel.Setup(m => m.GetDrinks(null, It.IsAny<List<string>>(), null, 0, 10, null))
                .Returns(drinks.FindAll(d =>
                    d.AlcoholContent >= 0 &&
                    d.AlcoholContent <= 10 &&
                    d.DrinkBrand.BrandName == "Ice"));

            var result = _drinkService.GetDrinks(null, new List<string> { "Ice" }, null, 0, 10, null);

            Assert.Single(result);
            Assert.Equal("Beer", result[0].DrinkName);
        }

        [Fact]
        public void AddDrink_ValidDrink_CallsModelAddDrink()
        {
            _drinkService.AddDrink("Lemonade", "lemon.png", _testCategories, "FreshCo", 0);
            _mockModel.Verify(m => m.AddDrink("Lemonade", "lemon.png", _testCategories, "FreshCo", 0), Times.Once);
        }

        [Fact]
        public void VoteDrinkOfTheDay_ValidVote_ReturnsVotedDrink()
        {
            var votedDrink = new Drink(10, "Iced Tea", "tea.jpg", _testCategories, _testBrand, 0);
            _mockModel.Setup(m => m.GetDrinkById(10)).Returns(votedDrink);

            var result = _drinkService.VoteDrinkOfTheDay(2, 10);

            _mockModel.Verify(m => m.VoteDrinkOfTheDay(2, 10), Times.Once);
            Assert.Equal("Iced Tea", result.DrinkName);
        }

        [Fact]
        public void IsDrinkInUserPersonalList_TrueAndFalseCases()
        {
            _mockModel.Setup(m => m.IsDrinkInPersonalList(1, 5)).Returns(true);
            _mockModel.Setup(m => m.IsDrinkInPersonalList(2, 5)).Returns(false);

            var result1 = _drinkService.IsDrinkInUserPersonalList(1, 5);
            var result2 = _drinkService.IsDrinkInUserPersonalList(2, 5);

            Assert.True(result1);
            Assert.False(result2);
        }

        [Fact]
        public void GetUserPersonalDrinkList_ReturnsList()
        {
            var personalList = new List<Drink> { _testDrink };
            _mockModel.Setup(m => m.GetPersonalDrinkList(1, 1)).Returns(personalList);

            var result = _drinkService.GetUserPersonalDrinkList(1);

            Assert.Single(result);
            Assert.Equal("Orange Juice", result[0].DrinkName);
        }

        [Fact]
        public void GetDrinkOfTheDay_ReturnsCorrectDrink()
        {
            _mockModel.Setup(m => m.GetDrinkOfTheDay()).Returns(_testDrink);

            var result = _drinkService.GetDrinkOfTheDay();

            Assert.Equal("Orange Juice", result.DrinkName);
        }

        [Fact]
        public void AddToUserPersonalDrinkList_Successful_ReturnsTrue()
        {
            _mockModel.Setup(m => m.AddToPersonalDrinkList(1, 1)).Returns(true);

            var result = _drinkService.AddToUserPersonalDrinkList(1, 1);

            Assert.True(result);
        }

        [Fact]
        public void DeleteFromUserPersonalDrinkList_Successful_ReturnsTrue()
        {
            _mockModel.Setup(m => m.DeleteFromPersonalDrinkList(1, 1)).Returns(true);

            var result = _drinkService.DeleteFromUserPersonalDrinkList(1, 1);

            Assert.True(result);
        }

        [Fact]
        public void UpdateDrink_CallsModelUpdate()
        {
            _drinkService.UpdateDrink(_testDrink);

            _mockModel.Verify(m => m.UpdateDrink(_testDrink), Times.Once);
        }

        [Fact]
        public void DeleteDrink_ValidId_CallsModelDeleteDrink()
        {
            _drinkService.DeleteDrink(1);

            _mockModel.Verify(m => m.DeleteDrink(1), Times.Once);
        }

       
        [Fact]
        public void GetDrinkCategories_ReturnsCategoryList()
        {
            var categories = new List<Category> { new Category(1, "Fizzy") };
            _mockModel.Setup(m => m.GetDrinkCategories()).Returns(categories);

            var result = _drinkService.GetDrinkCategories();

            Assert.Single(result);
            Assert.Equal("Fizzy", result[0].CategoryName);
        }

        [Fact]
        public void GetDrinkBrandNames_ReturnsBrandList()
        {
            var brands = new List<Brand> { new Brand(1, "ColaCo") };
            _mockModel.Setup(m => m.GetDrinkBrands()).Returns(brands);

            var result = _drinkService.GetDrinkBrandNames();

            Assert.Single(result);
            Assert.Equal("ColaCo", result[0].BrandName);
        }


    }
}
