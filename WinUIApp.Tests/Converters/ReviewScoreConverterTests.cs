using System;
using Xunit;
using WinUIApp.Utils.Converters;
using Microsoft.UI.Xaml.Media.Imaging;

namespace WinUIApp.Tests.Converters
{
    public class ReviewScoreConverterTests
    {
        private readonly ReviewScoreConverter _converter;

        public ReviewScoreConverterTests()
        {
            _converter = new ReviewScoreConverter();
        }

        [Fact]
        public void Convert_WithValidFloat_ReturnsFormattedReviewScore()
        {
            float sampleReviewScore = 4.5f;
            string expectedFormattedScore = "4.5/5";

            object conversionResult = _converter.Convert(sampleReviewScore, typeof(string), null, "en-US");

            //The result should not be null and should match the expected formatted string.
            Assert.NotNull(conversionResult);
            Assert.Equal(expectedFormattedScore, conversionResult.ToString());
        }

        [Fact]
        public void Convert_WithInvalidInput_ReturnsDefaultScoreDisplay()
        {
            object invalidInput = "This is not a float";
            string expectedDefaultDisplay = "N/A";

            object conversionResult = _converter.Convert(invalidInput, typeof(string), null, "en-US");

            //The converter should return the default display value.
            Assert.NotNull(conversionResult);
            Assert.Equal(expectedDefaultDisplay, conversionResult.ToString());
        }

        [Fact]
        public void Convert_WithNullInput_ReturnsDefaultScoreDisplay()
        {
            object nullInput = null;
            string expectedDefaultDisplay = "N/A";

            object conversionResult = _converter.Convert(nullInput, typeof(string), null, "en-US");

            Assert.NotNull(conversionResult);
            Assert.Equal(expectedDefaultDisplay, conversionResult.ToString());
        }

        [Fact]
        public void ConvertBack_ThrowsNotImplementedException()
        {
            object displayedReviewScoreValue = "4.5/5";
.
            var exception = Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack(displayedReviewScoreValue, typeof(float), null, "en-US")
            );
            Assert.Equal("Converting from string to review score is not supported.", exception.Message);
        }
    }
}
