using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

namespace WinUIApp.Utils.Converters
{
    public class CategoriesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is List<Category> categories && categories.Count > 0)
            {
                return string.Join(", ", categories.Select(c => c.CategoryName));
            }
            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
