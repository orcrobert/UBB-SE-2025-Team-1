using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class AlcoholContentFilterComponent : UserControl
    {
        // Constants for slider default values
        private const double MinimumAlcoholPercentage = 0;
        private const double MaximumAlcoholPercentage = 100;

        /// <summary>
        /// Gets or sets the minimum alcohol content percentage value.
        /// </summary>
        public double MinimumAlcoholContent
        {
            get => MinValueSlider.Value;
            set
            {
                MinValueSlider.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum alcohol content percentage value.
        /// </summary>
        public double MaximumAlcoholContent
        {
            get => MaxValueSlider.Value;
            set
            {
                MaxValueSlider.Value = value;
            }
        }

        /// <summary>
        /// Event that fires when the minimum alcohol content value changes.
        /// </summary>
        public event EventHandler<double> MinimumAlcoholContentChanged;

        /// <summary>
        /// Event that fires when the maximum alcohol content value changes.
        /// </summary>
        public event EventHandler<double> MaximumAlcoholContentChanged;

        /// <summary>
        /// Initializes a new instance of the AlcoholContentFilterComponent control.
        /// </summary>
        public AlcoholContentFilterComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles value changes for the minimum value slider, ensuring it doesn't exceed the maximum slider value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="valueChangedEventArgs">Event data for the value changed event.</param>
        private void MinValueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs valueChangedEventArgs)
        {
            if (MinValueSlider.Value > MaxValueSlider.Value)
            {
                MinValueSlider.Value = MinimumAlcoholPercentage;
                return;
            }
            MinimumAlcoholContentChanged?.Invoke(this, MinValueSlider.Value);
        }

        /// <summary>
        /// Handles value changes for the maximum value slider, ensuring it doesn't fall below the minimum slider value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="valueChangedEventArgs">Event data for the value changed event.</param>
        private void MaxValueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs valueChangedEventArgs)
        {
            if (MaxValueSlider.Value < MinValueSlider.Value)
            {
                MaxValueSlider.Value = MaximumAlcoholPercentage;
                return;
            }
            MaximumAlcoholContentChanged?.Invoke(this, MaxValueSlider.Value);
        }

        /// <summary>
        /// Resets the minimum value slider to its default value and triggers the change event.
        /// </summary>
        public void ResetMinSlider()
        {
            MinValueSlider.Value = MinimumAlcoholPercentage;
            MinimumAlcoholContentChanged?.Invoke(this, MinValueSlider.Value);
        }

        /// <summary>
        /// Resets the maximum value slider to its default value and triggers the change event.
        /// </summary>
        public void ResetMaxSlider()
        {
            MaxValueSlider.Value = MaximumAlcoholPercentage;
            MaximumAlcoholContentChanged?.Invoke(this, MaxValueSlider.Value);
        }

        /// <summary>
        /// Resets both minimum and maximum sliders to their default values.
        /// </summary>
        public void ResetSliders()
        {
            ResetMinSlider();
            ResetMaxSlider();
        }
    }

}
