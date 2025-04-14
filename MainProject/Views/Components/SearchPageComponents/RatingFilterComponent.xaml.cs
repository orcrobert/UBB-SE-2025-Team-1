// <copyright file="RatingFilterComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// Represents a user control for filtering drinks by rating in a search page.
    /// </summary>
    public sealed partial class RatingFilterComponent : UserControl
    {
        private const int MaximumStarRating = 5;
        private const string FiveStarsOption = "5 Stars";
        private const string FourStarsOption = "4 Stars";
        private const string ThreeStarsOption = "3 Stars";
        private const string TwoStarsOption = "2 Stars";
        private const string OneStarOption = "1 Star";

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingFilterComponent"/> class.
        /// </summary>
        public RatingFilterComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event that fires when the selected rating changes, providing the new rating value.
        /// </summary>
        public event EventHandler<float> RatingChanged;

        /// <summary>
        /// Sets the rating filter based on a string representation of the rating value.
        /// </summary>
        /// <param name="sortField">String representation of the rating to select.</param>
        public void SetSortBy(string sortField)
        {
            switch (sortField)
            {
                case FiveStarsOption:
                    this.RatingComboBox.SelectedIndex = 0;
                    break;
                case FourStarsOption:
                    this.RatingComboBox.SelectedIndex = 1;
                    break;
                case ThreeStarsOption:
                    this.RatingComboBox.SelectedIndex = 2;
                    break;
                case TwoStarsOption:
                    this.RatingComboBox.SelectedIndex = 3;
                    break;
                case OneStarOption:
                    this.RatingComboBox.SelectedIndex = 4;
                    break;
            }
        }

        /// <summary>
        /// Clears the current rating selection.
        /// </summary>
        public void ClearSelection()
        {
            this.RatingComboBox.SelectedItem = null;
        }

        /// <summary>
        /// Handles selection changes in the rating combo box by calculating the star rating
        /// and triggering the RatingChanged event with the updated value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="selectionChangedEventArgs">Event data for the selection changed event.</param>
        private void RatingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (this.RatingComboBox.SelectedItem is ComboBoxItem)
            {
                float selectedStarRating = MaximumStarRating - this.RatingComboBox.SelectedIndex;
                this.RatingChanged?.Invoke(this, selectedStarRating);
            }
        }
    }
}