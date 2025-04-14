using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Moq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServices;
using WinUIApp.ViewModels;
using Xunit;

namespace WinUIApp.Tests.ViewModels
{
    public class AddDrinkMenuViewModelTests
    {
        private readonly Mock<IDrinkService> _drinkServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IAdminService> _adminServiceMock;
        private readonly AddDrinkMenuViewModel _viewModel;

        public AddDrinkMenuViewModelTests()
        {
            _drinkServiceMock = new Mock<IDrinkService>();
            _userServiceMock = new Mock<IUserService>();
            _adminServiceMock = new Mock<IAdminService>();
            _viewModel = new AddDrinkMenuViewModel(
                _drinkServiceMock.Object,
                _userServiceMock.Object,
                _adminServiceMock.Object
            );
        }

        [Fact]
        public void Property_Setters_Should_Trigger_PropertyChanged()
        {
            var changedProperties = new List<string>();
            _viewModel.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName);

            _viewModel.DrinkName = "Test Drink";
            _viewModel.DrinkURL = "http://example.com/image.png";
            _viewModel.BrandName = "Test Brand";
            _viewModel.AlcoholContent = "5";

            Assert.Contains(nameof(_viewModel.DrinkName), changedProperties);
            Assert.Contains(nameof(_viewModel.DrinkURL), changedProperties);
            Assert.Contains(nameof(_viewModel.BrandName), changedProperties);
            Assert.Contains(nameof(_viewModel.AlcoholContent), changedProperties);
        }

        [Fact]
        public void GetSelectedCategories_Should_Return_Correct_Categories()
        {
            var category1 = new Category(1, "Beer");
            var category2 = new Category(2, "Wine");

            _viewModel.AllCategoryObjects = new List<Category> { category1, category2 };
            _viewModel.SelectedCategoryNames = new ObservableCollection<string> { "Beer" };

            var selected = _viewModel.GetSelectedCategories();

            Assert.Single(selected);
            Assert.Equal("Beer", selected.First().CategoryName);
        }

        [Fact]
        public void ValidateUserDrinkInput_Should_Throw_For_Invalid_Inputs()
        {
            // Empty Name
            _viewModel.DrinkName = string.Empty;
            _viewModel.BrandName = "Brand";
            _viewModel.AlcoholContent = "10";
            _viewModel.SelectedCategoryNames.Add("Beer");

            Assert.Throws<ArgumentException>(() => _viewModel.ValidateUserDrinkInput());

            // Empty Brand
            _viewModel.DrinkName = "Drink";
            _viewModel.BrandName = string.Empty;
            Assert.Throws<ArgumentException>(() => _viewModel.ValidateUserDrinkInput());

            // Invalid Alcohol Content
            _viewModel.BrandName = "Brand";
            _viewModel.AlcoholContent = "abc";
            Assert.Throws<ArgumentException>(() => _viewModel.ValidateUserDrinkInput());

            _viewModel.AlcoholContent = "-5";
            Assert.Throws<ArgumentException>(() => _viewModel.ValidateUserDrinkInput());

            _viewModel.AlcoholContent = "150";
            Assert.Throws<ArgumentException>(() => _viewModel.ValidateUserDrinkInput());

            // No Category Selected
            _viewModel.AlcoholContent = "5";
            _viewModel.SelectedCategoryNames.Clear();
            Assert.Throws<ArgumentException>(() => _viewModel.ValidateUserDrinkInput());
        }

        [Fact]
        public void ValidateUserDrinkInput_Should_Throw_When_No_Category_Selected()
        {
            // Arrange
            _viewModel.DrinkName = "Test Drink";
            _viewModel.BrandName = "Test Brand";
            _viewModel.AlcoholContent = "10";

            // Clear selected categories
            _viewModel.SelectedCategoryNames.Clear();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _viewModel.ValidateUserDrinkInput());
            Assert.Equal("At least one drinkCategory must be selected", exception.Message);
        }

        [Fact]
        public void ValidateUserDrinkInput_Should_Not_Throw_When_Category_Selected()
        {
            // Arrange
            _viewModel.DrinkName = "Test Drink";
            _viewModel.BrandName = "Test Brand";
            _viewModel.AlcoholContent = "10";
            _viewModel.SelectedCategoryNames = new ObservableCollection<string> { "Beer" };  // Ensure a category is selected

            // Act & Assert
            var exception = Record.Exception(() => _viewModel.ValidateUserDrinkInput());
            Assert.Null(exception);  // No exception should be thrown
        }

        [Fact]
        public void InstantAddDrink_Should_Call_DrinkService_AddDrink()
        {
            _viewModel.DrinkName = "Test Drink";
            _viewModel.DrinkURL = "http://example.com/image.png";
            _viewModel.BrandName = "Test Brand";
            _viewModel.AlcoholContent = "5";
            _viewModel.AllCategoryObjects = new List<Category>
        {
            new Category(1, "Beer")
        };
            _viewModel.SelectedCategoryNames.Add("Beer");

            _viewModel.InstantAddDrink();

            _drinkServiceMock.Verify(ds => ds.AddDrink(
                "Test Drink",
                "http://example.com/image.png",
                It.IsAny<List<Category>>(),
                "Test Brand",
                5f
            ), Times.Once);
        }

        [Fact]
        public void InstantAddDrink_Should_Throw_Exception_When_DrinkService_Fails()
        {
            // Arrange
            var categories = new List<Category>
        {
            new Category(1, "Beer")
        };
            _viewModel.DrinkName = "Test Drink";
            _viewModel.DrinkURL = "http://example.com/image.png";
            _viewModel.BrandName = "Test Brand";
            _viewModel.AlcoholContent = "5";
            _viewModel.SelectedCategoryNames.Add("Beer");

            // Simulate an exception when calling AddDrink
            _drinkServiceMock.Setup(ds => ds.AddDrink(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<List<Category>>(),
                It.IsAny<string>(),
                It.IsAny<float>()
            )).Throws(new Exception("Error adding drink"));

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _viewModel.InstantAddDrink());

            Assert.Equal("Error adding drink", exception.Message);
        }

        [Fact]
        public void SendAddDrinkRequest_Should_Call_AdminService_SendNotification()
        {
            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(123);
            _viewModel.DrinkName = "Test Drink";

            _viewModel.SendAddDrinkRequest();

            _adminServiceMock.Verify(admin => admin.SendNotificationFromUserToAdmin(
                123,
                "New Drink Request",
                "User requested to add new drink: Test Drink"
            ), Times.Once);
        }

        [Fact]
        public void SendAddDrinkRequest_Should_Throw_Exception_When_AdminService_Fails()
        {
            // Arrange
            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(123);
            _viewModel.DrinkName = "Test Drink";

            // Simulate an exception when calling SendNotificationFromUserToAdmin
            _adminServiceMock.Setup(admin => admin.SendNotificationFromUserToAdmin(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Throws(new Exception("Error sending notification"));

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _viewModel.SendAddDrinkRequest());

            Assert.Equal("Error sending notification", exception.Message);
        }

        [Fact]
        public void ClearForm_Should_Clear_All_Fields()
        {
            _viewModel.DrinkName = "Test";
            _viewModel.DrinkURL = "url";
            _viewModel.BrandName = "brand";
            _viewModel.AlcoholContent = "10";
            _viewModel.SelectedCategoryNames.Add("Category");

            _viewModel.ClearForm();

            Assert.Equal(string.Empty, _viewModel.DrinkName);
            Assert.Equal(string.Empty, _viewModel.DrinkURL);
            Assert.Equal(string.Empty, _viewModel.BrandName);
            Assert.Equal(string.Empty, _viewModel.AlcoholContent);
            Assert.Empty(_viewModel.SelectedCategoryNames);
        }
    }

}
