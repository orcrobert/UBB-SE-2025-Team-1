using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

namespace WinUIApp.Utils.Converters
{
    public class CategoriesConverter : IValueConverter
    {
        private const string DefaultCategoryDisplay = "N/A";
        private const string Separator = ", ";
        public object Convert(object drinkCategorySourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (drinkCategorySourceValue is List<Category> drinkCategories && drinkCategories.Any())
            {
                return string.Join(Separator, drinkCategories.Select(drinkCategory => drinkCategory.Name));
            }

            return DefaultCategoryDisplay;
        }

        public object ConvertBack(object displayedDrinkCategoryValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from string to category list is not supported.");
        }
    }
}
