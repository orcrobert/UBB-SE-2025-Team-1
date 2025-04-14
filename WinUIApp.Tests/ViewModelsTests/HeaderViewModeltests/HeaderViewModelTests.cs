using Moq;
using System.Collections.Generic;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Views.ViewModels;
using Xunit;

namespace WinUIApp.Tests.ViewModels
{
    public class HeaderViewModelTests
    {
        [Fact]
        public void GetCategories_ReturnsCorrectList()
        {
            // Arrange
            var expectedCategories = new List<Category>
            {
                new Category(1, "Beer"),
                new Category(2, "Wine")
            };

            var mockService = new Mock<IDrinkService>();
            mockService.Setup(s => s.GetDrinkCategories()).Returns(expectedCategories);

            var viewModel = new HeaderViewModel(mockService.Object);

            // Act
            var result = viewModel.GetCategories();

            // Assert
            Assert.Equal(expectedCategories, result);
            mockService.Verify(s => s.GetDrinkCategories(), Times.Once);
        }
    }
}
