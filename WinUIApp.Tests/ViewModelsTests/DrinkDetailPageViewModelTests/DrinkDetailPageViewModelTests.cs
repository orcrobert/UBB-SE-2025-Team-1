using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServices;
using WinUIApp.Views.ViewModels;
using Xunit;

namespace WinUIApp.Tests.ViewModels
{
    public class DrinkDetailPageViewModelTests
    {
        private readonly Mock<IDrinkService> _drinkServiceMock;
        private readonly Mock<IDrinkReviewService> _reviewServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IAdminService> _adminServiceMock;
        private readonly DrinkDetailPageViewModel _viewModel;

        public DrinkDetailPageViewModelTests()
        {
            _drinkServiceMock = new Mock<IDrinkService>();
            _reviewServiceMock = new Mock<IDrinkReviewService>();
            _userServiceMock = new Mock<IUserService>();
            _adminServiceMock = new Mock<IAdminService>();

            _viewModel = new DrinkDetailPageViewModel(
                _drinkServiceMock.Object,
                _reviewServiceMock.Object,
                _userServiceMock.Object,
                _adminServiceMock.Object
            );
        }

        [Fact]
        public void CategoriesDisplay_Should_Return_Correct_String()
        {
            var drink = new Drink(
                2,
                "Test Drink",
                "image.png",
                new List<Category>
                {
                    new Category(1, "Beer"),
                    new Category(2, "Wine")
                },
                new Brand(1, "Brand A"),
                12.5f
            );

            _viewModel.Drink = drink;

            Assert.Equal("Beer, Wine", _viewModel.CategoriesDisplay);
        }
        [Fact]
        public void CategoriesDisplay_Should_Return_Empty_String_When_No_Categories()
        {
            // Test when the category list is null
            var drinkWithNullCategories = new Drink(
                3,
                "Drink with No Categories",
                "image.png",
                null,
                new Brand(2, "Brand B"),
                7f
            );
            _viewModel.Drink = drinkWithNullCategories;
            Assert.Equal(string.Empty, _viewModel.CategoriesDisplay);

            // Test when the category list is an empty list
            var drinkWithEmptyCategories = new Drink(
                4,
                "Drink with Empty Categories",
                "image.png",
                new List<Category>(),
                new Brand(3, "Brand C"),
                8f
            );
            _viewModel.Drink = drinkWithEmptyCategories;
            Assert.Equal(string.Empty, _viewModel.CategoriesDisplay);
        }

        [Fact]
        public void CategoriesDisplay_Should_Return_Empty_String_When_Drink_Is_Null()
        {
            _viewModel.Drink = null;
            Assert.Equal(string.Empty, _viewModel.CategoriesDisplay);
        }
        [Fact]
        public void CategoriesDisplay_Should_Return_Empty_String_When_CategoryList_Is_Null()
        {
            var drinkWithNullCategories = new Drink(
                5,
                "Drink with Null CategoryList",
                "image.png",
                null, // Null CategoryList
                new Brand(1, "Brand A"),
                10f
            );
            _viewModel.Drink = drinkWithNullCategories;
            Assert.Equal(string.Empty, _viewModel.CategoriesDisplay);
        }


        [Fact]
        public void LoadDrink_Should_Load_Drink_Details_And_Reviews()
        {
            var drink = new Drink(
                10,
                "Strong Drink",
                "strong.jpg",
                new List<Category>(),
                new Brand(3, "HeavyBrand"),
                40f
            );

            var reviews = new List<Review>
            {
                new Review(10, 4.0f, 1, "Nice", "Pretty good", DateTime.UtcNow),
                new Review(10, 5.0f, 2, "Excellent", "Loved it!", DateTime.UtcNow)
            };

            _drinkServiceMock.Setup(ds => ds.GetDrinkById(10)).Returns(drink);
            _reviewServiceMock.Setup(rs => rs.GetReviewAverageByID(10)).Returns(4.5f);
            _reviewServiceMock.Setup(rs => rs.GetReviewsByID(10)).Returns(reviews);

            _viewModel.LoadDrink(10);

            Assert.Equal(drink, _viewModel.Drink);
            Assert.Equal(4.5f, _viewModel.AverageReviewScore);
            Assert.Equal(2, _viewModel.Reviews.Count);
        }

        [Fact]
        public void IsCurrentUserAdmin_Should_Return_Correct_Result()
        {
            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(101);
            _adminServiceMock.Setup(a => a.IsAdmin(101)).Returns(true);

            var result = _viewModel.IsCurrentUserAdmin();

            Assert.True(result);
        }

        [Fact]
        public void RemoveDrink_Should_Delete_If_Admin()
        {
            var drink = new Drink(
                5,
                "Removable Drink",
                "image.jpg",
                new List<Category>(),
                new Brand(1, "RemovableBrand"),
                6.9f
            );
            _viewModel.Drink = drink;

            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(1);
            _adminServiceMock.Setup(a => a.IsAdmin(1)).Returns(true);

            _viewModel.RemoveDrink();

            _drinkServiceMock.Verify(ds => ds.DeleteDrink(5), Times.Once);
        }

        [Fact]
        public void RemoveDrink_Should_Notify_Admin_If_Not_Admin()
        {
            var drink = new Drink(
                8,
                "User Drink",
                "user.jpg",
                new List<Category>(),
                new Brand(2, "UserBrand"),
                7.7f
            );
            _viewModel.Drink = drink;

            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(55);
            _adminServiceMock.Setup(a => a.IsAdmin(55)).Returns(false);

            _viewModel.RemoveDrink();

            _adminServiceMock.Verify(admin => admin.SendNotificationFromUserToAdmin(
                55,
                "Removal of drink with id:8 and name:User Drink",
                "User requested removal of drink from database."
            ), Times.Once);
        }

        [Fact]
        public void VoteForDrink_Should_Call_VoteDrinkOfTheDay()
        {
            var drink = new Drink(
                9,
                "VoteMe",
                "voteme.jpg",
                new List<Category>(),
                new Brand(4, "VoteBrand"),
                11f
            );
            _viewModel.Drink = drink;

            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(77);

            _viewModel.VoteForDrink();

            _drinkServiceMock.Verify(ds => ds.VoteDrinkOfTheDay(77, 9), Times.Once);
        }

        [Fact]
        public void VoteForDrink_Should_Throw_Exception_When_VoteDrinkOfTheDay_Fails()
        {
            // Arrange
            var drink = new Drink(
                9,
                "VoteMe",
                "voteme.jpg",
                new List<Category>(),
                new Brand(4, "VoteBrand"),
                11f
            );
            _viewModel.Drink = drink;

            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(77);

            // Simulate an exception when VoteDrinkOfTheDay is called
            _drinkServiceMock.Setup(ds => ds.VoteDrinkOfTheDay(77, 9))
                             .Throws(new Exception("Vote failed"));

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _viewModel.VoteForDrink());
            Assert.Equal("Error happened while voting for a drink:", exception.Message);
            Assert.IsType<Exception>(exception.InnerException);
            Assert.Equal("Vote failed", exception.InnerException.Message);
        }
        

    }
}
