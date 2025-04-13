using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class RatingFilterComponent : UserControl
    {
        private const int MaximumStarRating = 5;
        private const string FiveStarsOption = "5 Stars";
        private const string FourStarsOption = "4 Stars";
        private const string ThreeStarsOption = "3 Stars";
        private const string TwoStarsOption = "2 Stars";
        private const string OneStarOption = "1 Star";

        /// <summary>
        /// Event that fires when the selected rating changes, providing the new rating value.
        /// </summary>
        public event EventHandler<float> RatingChanged;

        /// <summary>
        /// Initializes a new instance of the RatingFilterComponent control.
        /// </summary>
        public RatingFilterComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles selection changes in the rating combo box by calculating the star rating
        /// and triggering the RatingChanged event with the updated value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="selectionChangedEventArgs">Event data for the selection changed event.</param>
        private void RatingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (RatingComboBox.SelectedItem is ComboBoxItem)
            {
                float selectedStarRating = MaximumStarRating - RatingComboBox.SelectedIndex;
                RatingChanged?.Invoke(this, selectedStarRating);
            }
        }

        /// <summary>
        /// Sets the rating filter based on a string representation of the rating value.
        /// </summary>
        /// <param name="sortField">String representation of the rating to select.</param>
        public void SetSortBy(string sortField)
        {
            switch (sortField)
            {
                case FiveStarsOption:
                    RatingComboBox.SelectedIndex = 0;
                    break;
                case FourStarsOption:
                    RatingComboBox.SelectedIndex = 1;
                    break;
                case ThreeStarsOption:
                    RatingComboBox.SelectedIndex = 2;
                    break;
                case TwoStarsOption:
                    RatingComboBox.SelectedIndex = 3;
                    break;
                case OneStarOption:
                    RatingComboBox.SelectedIndex = 4;
                    break;
            }
        }

        /// <summary>
        /// Clears the current rating selection.
        /// </summary>
        public void ClearSelection()
        {
            RatingComboBox.SelectedItem = null;
        }
    }
}
