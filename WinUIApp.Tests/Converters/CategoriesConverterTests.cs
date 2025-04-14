using System;
using System.Collections.Generic;
using Xunit;
using WinUIApp.Utils.Converters;
using WinUIApp.Models;

namespace WinUIApp.Tests.Converters
{
    public class CategoriesConverterTests
    {
        private readonly CategoriesConverter _converter;

        public CategoriesConverterTests()
        {
            _converter = new CategoriesConverter();
        }

        [Fact]
        public void Convert_WithValidCategoryList_ReturnsCommaSeparatedCategoryNames()
        {
            var sampleCategories = new List<Category>
            {
                new Category(1, "Fruit"),
                new Category(2, "Soda"),
                new Category(3, "Juice")
            };
            string expectedOutput = "Fruit, Soda, Juice";

            object conversionResult = _converter.Convert(sampleCategories, typeof(string), null, "en-US");

            Assert.NotNull(conversionResult);
            Assert.Equal(expectedOutput, conversionResult.ToString());
        }

        [Fact]
        public void Convert_WithEmptyCategoryList_ReturnsDefaultDisplay()
        {
            var emptyCategoryList = new List<Category>(); // No categories selected
            string expectedOutput = "N/A"; // Matches the DefaultDrinkCategoriesDisplay constant

            object conversionResult = _converter.Convert(emptyCategoryList, typeof(string), null, "en-US");

            Assert.NotNull(conversionResult);
            Assert.Equal(expectedOutput, conversionResult.ToString());
        }

        [Fact]
        public void Convert_WithInvalidInput_ReturnsDefaultDisplay()
        {
            object invalidInput = "Not a list of categories";
            string expectedOutput = "N/A"; // The converter should return the default when input is invalid

            object conversionResult = _converter.Convert(invalidInput, typeof(string), null, "en-US");

            Assert.NotNull(conversionResult);
            Assert.Equal(expectedOutput, conversionResult.ToString());
        }

        [Fact]
        public void Convert_WithNullInput_ReturnsDefaultDisplay()
        {
            object nullInput = null;
            string expectedOutput = "N/A"; // Should return the default string

            object conversionResult = _converter.Convert(nullInput, typeof(string), null, "en-US");

            Assert.NotNull(conversionResult);
            Assert.Equal(expectedOutput, conversionResult.ToString());
        }

        [Fact]
        public void ConvertBack_AlwaysThrowsNotImplementedException()
        {
            object sampleDisplayValue = "Fruit, Soda, Juice";

            var exception = Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack(sampleDisplayValue, typeof(List<Category>), null, "en-US")
            );
            Assert.Equal("Converting from a formatted categories string back to a list of Category objects is not supported.", exception.Message);
        }
    }
}
