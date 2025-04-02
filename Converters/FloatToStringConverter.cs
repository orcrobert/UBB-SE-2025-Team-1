using Microsoft.UI.Xaml.Data;
using System;

namespace WinUIApp.Converters
{
    public class FloatToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is float floatValue)
            {
                return floatValue.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stringValue && float.TryParse(stringValue, out float floatValue))
            {
                return floatValue;
            }
            return 0f;
        }
    }
} 