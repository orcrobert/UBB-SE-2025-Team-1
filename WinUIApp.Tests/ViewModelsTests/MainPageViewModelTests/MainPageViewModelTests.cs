using Moq;
using System.Collections.Generic;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServices;
using WinUIApp.ViewModels;
using Xunit;

namespace WinUIApp.Tests.ViewModels
{
    public class MainPageViewModelTests
    {
        private readonly Mock<IDrinkService> _mockDrinkService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly MainPageViewModel _viewModel;
        private readonly Drink _testDrink;
        private readonly List<Drink> _userDrinks;

        public MainPageViewModelTests()
        {
            _mockDrinkService = new Mock<IDrinkService>();
            _mockUserService = new Mock<IUserService>();

            _testDrink = new Drink(
                id: 1,
                drinkName: "Mocktail",
                imageUrl: "https://example.com/img.jpg",
                categories: new List<Category> { new Category(1, "Mocked") },
                brand: new Brand(99, "Mock Brand"),
                alcoholContent: 4.2f
            );

            _userDrinks = new List<Drink> { _testDrink };

            _mockDrinkService.Setup(s => s.GetDrinkOfTheDay()).Returns(_testDrink);
            _mockUserService.Setup(s => s.GetCurrentUserId()).Returns(42);
            _mockDrinkService.Setup(s => s.GetUserPersonalDrinkList(42, 5)).Returns(_userDrinks);

            _viewModel = new MainPageViewModel(_mockDrinkService.Object, _mockUserService.Object);
        }

        [Fact]
        public void Constructor_SetsDrinkOfTheDayPropertiesCorrectly()
        {
            Assert.Equal(_testDrink.DrinkImageUrl, _viewModel.ImageSource);
            Assert.Equal(_testDrink.DrinkName, _viewModel.DrinkName);
            Assert.Equal(_testDrink.DrinkBrand.BrandName, _viewModel.DrinkBrand);
            Assert.Equal(_testDrink.CategoryList, _viewModel.DrinkCategories);
            Assert.Equal(_testDrink.AlcoholContent, _viewModel.AlcoholContent);
        }

        [Fact]
        public void Constructor_LoadsPersonalDrinksCorrectly()
        {
            Assert.Equal(_userDrinks, _viewModel.PersonalDrinks);
        }

        [Fact]
        public void GetDrinkOfTheDayId_Should_Return_Correct_Id()
        {
            // Act
            var drinkOfTheDayId = _viewModel.GetDrinkOfTheDayId();

            // Assert
            Assert.Equal(_testDrink.DrinkId, drinkOfTheDayId);
        }

        [Fact]
        public void SetField_Should_Trigger_PropertyChanged_When_Value_Changes()
        {
            // Arrange
            var changedProperties = new List<string>();
            _viewModel.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName);

            // Act
            _viewModel.DrinkName = "New Drink Name"; // Assuming DrinkName uses SetField internally

            // Assert
            Assert.Contains(nameof(_viewModel.DrinkName), changedProperties);
        }

        [Fact]
        public void SetField_Should_Not_Trigger_PropertyChanged_When_Value_Does_Not_Change()
        {
            // Arrange
            var changedProperties = new List<string>();
            _viewModel.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName);

            // Act
            _viewModel.DrinkName = _viewModel.DrinkName; // Setting the same value

            // Assert
            Assert.Empty(changedProperties); // No change, so PropertyChanged should not be triggered
        }
    }
}
