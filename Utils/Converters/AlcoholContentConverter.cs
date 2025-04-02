using Microsoft.UI.Xaml.Data;
using System;

namespace WinUIApp.Utils.Converters
{
    public class AlcoholContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is float alcoholContent)
            {
                return $"{alcoholContent}%";
            }
            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
