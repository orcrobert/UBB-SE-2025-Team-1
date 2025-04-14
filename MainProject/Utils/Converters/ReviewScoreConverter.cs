// <copyright file="ReviewScoreConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Utils.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converter for displaying review scores in a user-friendly format.
    /// </summary>
    public class ReviewScoreConverter : IValueConverter
    {
        private const string ScoreFormat = "{0:F1}/5";
        private const string DefaultScoreDisplay = "N/A";

        /// <summary>
        /// Converts a review score to a formatted string for display.
        /// </summary>
        /// <param name="reviewScoreSourceValue">Value.</param>
        /// <param name="destinationType">Type.</param>
        /// <param name="converterParameter">Parameter.</param>
        /// <param name="formattingCulture">Culture.</param>
        /// <returns>Object.</returns>
        public object Convert(object reviewScoreSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (reviewScoreSourceValue is float score)
            {
                return string.Format(ScoreFormat, score);
            }

            return DefaultScoreDisplay;
        }

        /// <summary>
        /// Converts back from a formatted string to a review score.
        /// </summary>
        /// <param name="displayedreviewScoreValue">Value.</param>
        /// <param name="sourcePropertyType">Type.</param>
        /// <param name="converterParameter">Parameter.</param>
        /// <param name="formattingCulture">Culture.</param>
        /// <returns>Object.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        public object ConvertBack(object displayedreviewScoreValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from string to review score is not supported.");
        }
    }
}