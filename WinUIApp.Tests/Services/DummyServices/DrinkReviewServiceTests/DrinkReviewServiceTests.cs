using WinUIApp.Services.DummyServices;
using Xunit;
using System.Linq;

namespace WinUIApp.Tests.Services.DummyServices.DrinkReviewServiceTests
{
    public class DrinkReviewServiceTests
    {
        private readonly DrinkReviewService _reviewService;

        public DrinkReviewServiceTests()
        {
            _reviewService = new DrinkReviewService();
        }

        [Fact]
        public void GetReviewsByID_WithExistingDrinkId_ReturnsCorrectReviews()
        {
            // Arrange
            int drinkId = 1;

            // Act
            var reviews = _reviewService.GetReviewsByID(drinkId);

            // Assert
            Assert.All(reviews, review => Assert.Equal(drinkId, review.DrinkId));
            Assert.NotEmpty(reviews);
        }

        [Fact]
        public void GetReviewsByID_WithNonExistingDrinkId_ReturnsEmptyList()
        {
            // Arrange
            int drinkId = 9999;

            // Act
            var reviews = _reviewService.GetReviewsByID(drinkId);

            // Assert
            Assert.Empty(reviews);
        }

        [Fact]
        public void GetReviewAverageByID_WithReviews_ReturnsCorrectAverage()
        {
            // Arrange
            int drinkId = 1;
            var reviews = _reviewService.GetReviewsByID(drinkId);
            float expectedAverage = reviews.Average(r => r.ReviewScore);

            // Act
            float actualAverage = _reviewService.GetReviewAverageByID(drinkId);

            // Assert
            Assert.Equal(expectedAverage, actualAverage, precision: 2);
        }

        [Fact]
        public void GetReviewAverageByID_WithNoReviews_ReturnsZero()
        {
            // Arrange
            int drinkId = 9999;

            // Act
            float result = _reviewService.GetReviewAverageByID(drinkId);

            // Assert
            Assert.Equal(0, result);
        }
    }
}
