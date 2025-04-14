using System;
using Xunit;
using Moq;
using WinUIApp.Utils.Converters;
using Microsoft.UI.Xaml.Media.Imaging;

namespace WinUIApp.Tests.Converters
{
    public class ImagePathConverterTests
    {
        private readonly ImagePathConverter _converter;
        private readonly Mock<IBitmapImageFactory> _factoryMock;

        private const string ValidImageUrl = "https://via.placeholder.com/150";
        private const string InvalidImageUrl = "invalid-url";
        private const string FallbackImagePath = "ms-appx:///Assets/DefaultDrink.jpg";

        public ImagePathConverterTests()
        {
            _factoryMock = new Mock<IBitmapImageFactory>();

            _factoryMock
                .Setup(f => f.Create(It.IsAny<string>()))
                .Returns(() => default(BitmapImage)); // This avoids COM exception

            //Injects the mock into the converter
            _converter = new ImagePathConverter(_factoryMock.Object);
        }

        [Fact]
        public void Convert_WithValidImageUrl_CallsFactoryWithValidUrl()
        {
            _converter.Convert(ValidImageUrl, typeof(object), null, "en-US");

            _factoryMock.Verify(f => f.Create(ValidImageUrl), Times.Once);
        }

        [Fact]
        public void Convert_WithEmptyOrNullImageUrl_CallsFactoryWithFallbackPath()
        {
            _converter.Convert("", typeof(object), null, "en-US");
            _converter.Convert(null, typeof(object), null, "en-US");

            _factoryMock.Verify(f => f.Create(FallbackImagePath), Times.Exactly(2));
        }

        [Fact]
        public void Convert_WithInvalidImageUrl_CallsFactoryWithFallbackPath()
        {
            _factoryMock
                .Setup(f => f.Create(InvalidImageUrl))
                .Throws(new Exception("Invalid URI"));

            _converter.Convert(InvalidImageUrl, typeof(object), null, "en-US");

            _factoryMock.Verify(f => f.Create(FallbackImagePath), Times.Once);
        }

        [Fact]
        public void ConvertBack_ThrowsNotImplementedException()
        {
            var exception = Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack(new object(), typeof(string), null, "en-US")
            );
            Assert.Equal("Converting from BitmapImage to URL string is not supported.", exception.Message);
        }
    }
}
