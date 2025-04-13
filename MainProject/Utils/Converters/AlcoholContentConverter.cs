using Microsoft.UI.Xaml.Data;
using System;

namespace WinUIApp.Utils.Converters
{
    public partial class AlcoholContentConverter : IValueConverter
    {
        private const string DefaultAlcoholPercentage = "0%";
        private const string AlcoholPercentageFormat = "{0}%";
        public object Convert(object alcoholContentSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (alcoholContentSourceValue is float alcoholContent)
            {
                return string.Format(AlcoholPercentageFormat, alcoholContent);
            }
            return DefaultAlcoholPercentage;
        }

        public object ConvertBack(object displayedAlcoholContentValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from string to alcohol content is not supported.");
        }
    }
}
