using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServices;
using WinUIApp.ViewModels;
using Xunit;

namespace WinUIApp.Tests.ViewModels
{
    public class UpdateDrinkMenuViewModelTests
    {
        private readonly Mock<IDrinkService> drinkServiceMock;
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<IAdminService> adminServiceMock;
        private readonly Drink testDrink;
        private readonly UpdateDrinkMenuViewModel viewModel;

        public UpdateDrinkMenuViewModelTests()
        {
            this.drinkServiceMock = new Mock<IDrinkService>();
            this.userServiceMock = new Mock<IUserService>();
            this.adminServiceMock = new Mock<IAdminService>();

            this.testDrink = new Drink
            (1, "Old Drink", "http://oldurl.com", new List<Category>(), new Brand(1, "OldBrand"), 5.0f);

            this.viewModel = new UpdateDrinkMenuViewModel(
                this.testDrink,
                this.drinkServiceMock.Object,
                this.userServiceMock.Object,
                this.adminServiceMock.Object);
        }
        [Fact]
        public void Property_Setters_Should_Trigger_PropertyChanged()
        {
            var changedProperties = new List<string>();
            viewModel.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName);

            viewModel.DrinkName = "Updated Drink";
            viewModel.DrinkURL = "http://example.com/image.png";
            viewModel.BrandName = "Updated Brand";
            viewModel.AlcoholContent = "10";

            Assert.Contains(nameof(viewModel.DrinkName), changedProperties);
            Assert.Contains(nameof(viewModel.DrinkURL), changedProperties);
            Assert.Contains(nameof(viewModel.BrandName), changedProperties);
            Assert.Contains(nameof(viewModel.AlcoholContent), changedProperties);
        }

        [Fact]
        public void GetSelectedCategories_ShouldReturnCorrectCategories()
        {
            var category1 = new Category(1, "Category1");
            var category2 = new Category(2, "Category2");

            viewModel.AllCategoryObjects = new List<Category> { category1, category2 };
            viewModel.SelectedCategoryNames = new ObservableCollection<string> { "Category1" };

            var selected = viewModel.GetSelectedCategories();
            Assert.Single(selected);
            Assert.Equal("Category1", selected.First().CategoryName);
        }

        [Fact]
        public void ValidateUpdatedDrinkDetails_ShouldThrow_WhenDrinkNameIsEmpty()
        {
            viewModel.DrinkName = "";
            var ex = Assert.Throws<ArgumentException>(() => viewModel.ValidateUpdatedDrinkDetails());
            Assert.Contains("Drink name is required", ex.Message);
        }

        [Fact]
        public void ValidateUpdatedDrinkDetails_ShouldThrow_WhenBrandDoesNotExist()
        {
            viewModel.BrandName = "NonExistentBrand";
            viewModel.AllBrands = new List<Brand> { new Brand(1, "SomeOtherBrand") };
            var ex = Assert.Throws<ArgumentException>(() => viewModel.ValidateUpdatedDrinkDetails());
            Assert.Contains("does not exist", ex.Message);
        }

        [Fact]
        public void ValidateUpdatedDrinkDetails_ShouldThrow_WhenNoCategoriesSelected()
        {
            viewModel.AllBrands = new List<Brand> { new Brand(1, viewModel.BrandName) };
            viewModel.SelectedCategoryNames.Clear();
            var ex = Assert.Throws<ArgumentException>(() => viewModel.ValidateUpdatedDrinkDetails());
            Assert.Contains("At least one category must be selected", ex.Message);
        }


        [Fact]
        public void InstantUpdateDrink_ShouldUpdateDrink_WhenSuccessful()
        {
            // Arrange
            var validBrand = new Brand(1, "NewBrand");
            var category1 = new Category(1, "Category1");
            viewModel.AllCategoryObjects = new List<Category> { category1 };
            viewModel.SelectedCategoryNames = new ObservableCollection<string> { "Category1" };

            // Mock the FindBrandByName method to return a valid brand
            this.drinkServiceMock.Setup(service => service.GetDrinkBrandNames()).Returns(new List<Brand> { validBrand });
            this.viewModel.BrandName = "NewBrand";  // Set the brand name

            this.drinkServiceMock.Setup(service => service.UpdateDrink(It.IsAny<Drink>())).Verifiable();

            // Act
            viewModel.InstantUpdateDrink();

            // Assert
            this.drinkServiceMock.Verify(service => service.UpdateDrink(It.IsAny<Drink>()), Times.Once);
        }


        [Fact]
        public void InstantUpdateDrink_ShouldHandleException_WhenUpdateFails()
        {
            // Arrange
            this.drinkServiceMock.Setup(service => service.UpdateDrink(It.IsAny<Drink>())).Throws(new Exception("Update failed"));

            // Act & Assert
            var ex = Record.Exception(() => viewModel.InstantUpdateDrink());
            Assert.Null(ex); // Check that no unhandled exception is thrown
        }

        [Fact]
        public void SendUpdateDrinkRequest_ShouldSendRequest_WhenSuccessful()
        {
            // Arrange
            this.adminServiceMock.Setup(service => service.SendNotificationFromUserToAdmin(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            // Act
            viewModel.SendUpdateDrinkRequest();

            // Assert
            this.adminServiceMock.Verify(service => service.SendNotificationFromUserToAdmin(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SendUpdateDrinkRequest_ShouldHandleException_WhenRequestFails()
        {
            // Arrange
            this.adminServiceMock.Setup(service => service.SendNotificationFromUserToAdmin(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Request failed"));

            // Act & Assert
            var ex = Record.Exception(() => viewModel.SendUpdateDrinkRequest());
            Assert.Null(ex); // Check that no unhandled exception is thrown
        }

        [Fact]
        public void ValidateUpdatedDrinkDetails_ShouldThrow_WhenDrinkNameIsNullOrWhiteSpace()
        {
            // Arrange
            viewModel.DrinkName = "  "; // or null
            var ex = Assert.Throws<ArgumentException>(() => viewModel.ValidateUpdatedDrinkDetails());

            // Assert
            Assert.Contains("Drink name is required", ex.Message);
        }

        [Fact]
        public void ValidateUpdatedDrinkDetails_ShouldUpdateDrinkBrand_WhenValidBrandName()
        {
            // Arrange
            var validBrand = new Brand(1, "ValidBrand");
            viewModel.AllBrands = new List<Brand> { validBrand };
            viewModel.DrinkName = "ValidDrink";
            viewModel.BrandName = "ValidBrand";

            // Add a selected category to avoid validation exception
            viewModel.SelectedCategoryNames = new ObservableCollection<string> { "Category1" };

            // Act
            viewModel.ValidateUpdatedDrinkDetails();

            // Assert
            Assert.Equal(validBrand, viewModel.DrinkToUpdate.DrinkBrand);
        }

    }

}
