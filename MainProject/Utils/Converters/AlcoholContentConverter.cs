// <copyright file="AlcoholContentConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Utils.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converter for displaying alcohol content in a user-friendly format.
    /// </summary>
    public partial class AlcoholContentConverter : IValueConverter
    {
        private const string DefaultAlcoholPercentage = "0%";
        private const string AlcoholPercentageFormat = "{0}%";

        /// <summary>
        /// Converts the alcohol content from a float to a string representation.
        /// </summary>
        /// <param name="alcoholContentSourceValue">Value.</param>
        /// <param name="destinationType">Type.</param>
        /// <param name="converterParameter">Parameters.</param>
        /// <param name="formattingCulture">Culture.</param>
        /// <returns>.</returns>
        public object Convert(object alcoholContentSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (alcoholContentSourceValue is float alcoholContent)
            {
                return string.Format(AlcoholPercentageFormat, alcoholContent);
            }

            return DefaultAlcoholPercentage;
        }

        /// <summary>
        /// Converts the alcohol content back from a string to a float representation.
        /// </summary>
        /// <param name="displayedAlcoholContentValue">Value.</param>
        /// <param name="sourcePropertyType">Type.</param>
        /// <param name="converterParameter">Parameters.</param>
        /// <param name="formattingCulture">Culture.</param>
        /// <returns>Object.</returns>
        /// <exception cref="NotImplementedException">Any issues.</exception>
        public object ConvertBack(object displayedAlcoholContentValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from string to alcohol content is not supported.");
        }
    }
}