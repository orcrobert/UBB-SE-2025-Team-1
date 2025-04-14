using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

namespace WinUIApp.Utils.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        private const string FallbackImagePath = "ms-appx:///Assets/DefaultDrink.jpg";
        private readonly IBitmapImageFactory _bitmapImageFactory;

        public ImagePathConverter() : this(new DefaultBitmapImageFactory()) { }

        public ImagePathConverter(IBitmapImageFactory bitmapImageFactory)
        {
            _bitmapImageFactory = bitmapImageFactory;
        }

        public object Convert(object imagePathSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (imagePathSourceValue is string url && !string.IsNullOrEmpty(url))
            {
                try
                {
                    return _bitmapImageFactory.Create(url);
                }
                catch (Exception)
                {
                    // If creation fails, fallback to the default image.
                    return _bitmapImageFactory.Create(FallbackImagePath);
                }
            }
            return _bitmapImageFactory.Create(FallbackImagePath);
        }

        public object ConvertBack(object displayedImagePathValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from BitmapImage to URL string is not supported.");
        }
    }
}
