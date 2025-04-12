using Microsoft.UI.Xaml.Data;
using System;

namespace WinUIApp.Utils.Converters
{
    public class ReviewScoreConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is float score)
            {
                return $"{score:F1}/5"; // e.g., "4.5/5"
            }
            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
