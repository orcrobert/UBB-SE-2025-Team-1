// <copyright file="CategoriesConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Utils.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.UI.Xaml.Data;
    using WinUIApp.Models;

    /// <summary>
    /// Converter to transform a list of categories into a comma-separated string.
    /// </summary>
    public class CategoriesConverter : IValueConverter
    {
        /// <summary>
        /// Converts a list of categories to a comma-separated string.
        /// </summary>
        /// <param name="value">value.</param>
        /// <param name="targetType">Type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="language">Language.</param>
        /// <returns>Object.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)

        {
            if (drinkCategoriesSourceValue is List<Category> drinkCategories && drinkCategories.Count > 0)
            {
                return string.Join(DrinkCategoriesSeparator, drinkCategories.Select(category => category.CategoryName));
            }
            return DefaultDrinkCategoriesDisplay;
        }

        /// <summary>
        /// Converts back the value. Not implemented.
        /// </summary>
        /// <param name="value">value.</param>
        /// <param name="targetType">Type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="language">Language.</param>
        /// <returns>Object.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)

        {
            throw new NotImplementedException("Converting from a formatted categories string back to a list of Category objects is not supported.");
        }
    }
}