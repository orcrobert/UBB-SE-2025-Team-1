// <copyright file="ImagePathConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Utils.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Converter for converting image paths to BitmapImage objects.
    /// </summary>
    public class ImagePathConverter : IValueConverter
    {
        private const string FallbackImagePath = "ms-appx:///Assets/DefaultDrink.png";

        /// <summary>
        /// Converts a string image path to a BitmapImage.
        /// </summary>
        /// <param name="imagePathSourceValue">Value.</param>
        /// <param name="destinationType">Type.</param>
        /// <param name="converterParameter">Parameter.</param>
        /// <param name="formattingCulture">Culture.</param>
        /// <returns>Object.</returns>
        public object Convert(object imagePathSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (imagePathSourceValue is string url && !string.IsNullOrEmpty(url))
            {
                BitmapImage bitmapImage = new BitmapImage();
                try
                {
                    bitmapImage = new BitmapImage(new Uri(url));
                    return bitmapImage;
                }
                catch (Exception)
                {
                    return new BitmapImage(new Uri(FallbackImagePath));
                }
            }

            return new BitmapImage(new Uri(FallbackImagePath));
        }

        /// <summary>
        /// Converts a BitmapImage back to a string image path. This is not supported in this converter.
        /// </summary>
        /// <param name="displayedImagePathValue">Value.</param>
        /// <param name="sourcePropertyType">Type.</param>
        /// <param name="converterParameter">Parameter.</param>
        /// <param name="formattingCulture">Culture.</param>
        /// <returns>Object.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        public object ConvertBack(object displayedImagePathValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from BitmapImage to URL string is not supported.");
        }
    }
}