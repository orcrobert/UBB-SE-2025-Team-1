using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace WinUIApp.Utils.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        private const string FallbackImagePath = "ms-appx:///Assets/DefaultDrink.png";
        public object Convert(object imagePathSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (imagePathSourceValue is string url && !string.IsNullOrEmpty(url))
            {
                return new BitmapImage(new Uri(url));
            }
            return new BitmapImage(new Uri(FallbackImagePath));
        }

        public object ConvertBack(object displayedImagePathValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from BitmapImage to URL string is not supported.");
        }
    }
}
