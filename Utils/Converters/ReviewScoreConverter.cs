using Microsoft.UI.Xaml.Data;
using System;

namespace WinUIApp.Utils.Converters
{
    public class ReviewScoreConverter : IValueConverter
    {
        private const string ScoreFormat = "{0:F1}/5";
        private const string DefaultScoreDisplay = "N/A";
        public object Convert(object reviewScoreSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (reviewScoreSourceValue is float score)

            {
                return string.Format(ScoreFormat, score);
            }
            return DefaultScoreDisplay;
        }

        public object ConvertBack(object displayedreviewScoreValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from string to review score is not supported.");
        }
    }
}
