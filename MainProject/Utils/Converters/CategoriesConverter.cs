using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

namespace WinUIApp.Utils.Converters
{
    public class CategoriesConverter : IValueConverter
    {
        private const string DefaultDrinkCategoriesDisplay = "N/A";
        private const string DrinkCategoriesSeparator = ", ";
        public object Convert(object drinkCategoriesSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (drinkCategoriesSourceValue is List<Category> drinkCategories && drinkCategories.Count > 0)
            {
                return string.Join(DrinkCategoriesSeparator, drinkCategories.Select(category => category.CategoryName));
            }
            return DefaultDrinkCategoriesDisplay;
        }

        public object ConvertBack(object drinkCategoriesContentValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from a formatted categories string back to a list of Category objects is not supported.");
        }
    }
}
