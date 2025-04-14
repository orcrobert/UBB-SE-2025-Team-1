using System;
using Xunit;
using WinUIApp.Utils.Converters;

namespace WinUIApp.Tests.Converters
{
    public class AlcoholContentConverterTests
    {
        private readonly AlcoholContentConverter _converter;

        public AlcoholContentConverterTests()
        {
            _converter = new AlcoholContentConverter();
        }

        [Fact]
        public void Convert_WithValidFloat_ReturnsFormattedPercentage()
        {
            float sampleAlcoholContent = 5.5f;
            string expectedFormattedPercentage = "5.5%";

            var result = _converter.Convert(sampleAlcoholContent, typeof(string), null, "en-US");

            Assert.NotNull(result);
            Assert.Equal(expectedFormattedPercentage, result.ToString());
        }

        [Fact]
        public void Convert_WithNonFloatValue_ReturnsDefaultAlcoholPercentage()
        {
            object invalidValue = "Not a float";
            string expectedDefaultPercentage = "0%";

            var result = _converter.Convert(invalidValue, typeof(string), null, "en-US");

            Assert.NotNull(result);
            Assert.Equal(expectedDefaultPercentage, result.ToString());
        }

        [Fact]
        public void Convert_WithNullValue_ReturnsDefaultAlcoholPercentage()
        {
            object nullValue = null;
            string expectedDefaultPercentage = "0%";

            var result = _converter.Convert(nullValue, typeof(string), null, "en-US");

            Assert.NotNull(result);
            Assert.Equal(expectedDefaultPercentage, result.ToString());
        }

        [Fact]
        public void ConvertBack_ThrowsNotImplementedException()
        {
            object sampleDisplayValue = "5.5%";

            Assert.Throws<NotImplementedException>(() => _converter.ConvertBack(sampleDisplayValue, typeof(float), null, "en-US"));
        }
    }
}
