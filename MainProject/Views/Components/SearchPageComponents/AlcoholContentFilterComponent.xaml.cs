// <copyright file="AlcoholContentFilterComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;

    /// <summary>
    /// Represents a user control for filtering drinks based on their alcohol content.
    /// </summary>
    public sealed partial class AlcoholContentFilterComponent : UserControl
    {
        private const double MinimumAlcoholPercentage = 0;
        private const double MaximumAlcoholPercentage = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlcoholContentFilterComponent"/> class.
        /// </summary>
        public AlcoholContentFilterComponent()
        {
            this.InitializeComponent();
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
        /// Gets or sets the minimum alcohol content percentage value.
        /// </summary>
        public double MinimumAlcoholContent
        {
            get => this.MinValueSlider.Value;
            set
            {
                this.MinValueSlider.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum alcohol content percentage value.
        /// </summary>
        public double MaximumAlcoholContent
        {
            get => this.MaxValueSlider.Value;
            set
            {
                this.MaxValueSlider.Value = value;
            }
        }

        /// <summary>
        /// Resets the minimum value slider to its default value and triggers the change event.
        /// </summary>
        public void ResetMinSlider()
        {
            this.MinValueSlider.Value = MinimumAlcoholPercentage;
            this.MinimumAlcoholContentChanged?.Invoke(this, this.MinValueSlider.Value);
        }

        /// <summary>
        /// Resets the maximum value slider to its default value and triggers the change event.
        /// </summary>
        public void ResetMaxSlider()
        {
            this.MaxValueSlider.Value = MaximumAlcoholPercentage;
            this.MaximumAlcoholContentChanged?.Invoke(this, this.MaxValueSlider.Value);
        }

        /// <summary>
        /// Resets both minimum and maximum sliders to their default values.
        /// </summary>
        public void ResetSliders()
        {
            this.ResetMinSlider();
            this.ResetMaxSlider();
        }

        /// <summary>
        /// Handles value changes for the minimum value slider, ensuring it doesn't exceed the maximum slider value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="valueChangedEventArgs">Event data for the value changed event.</param>
        private void MinValueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs valueChangedEventArgs)
        {
            if (this.MinValueSlider.Value > this.MaxValueSlider.Value)
            {
                this.MinValueSlider.Value = MinimumAlcoholPercentage;
                return;
            }

            this.MinimumAlcoholContentChanged?.Invoke(this, this.MinValueSlider.Value);
        }

        /// <summary>
        /// Handles value changes for the maximum value slider, ensuring it doesn't fall below the minimum slider value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="valueChangedEventArgs">Event data for the value changed event.</param>
        private void MaxValueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs valueChangedEventArgs)
        {
            if (this.MaxValueSlider.Value < this.MinValueSlider.Value)
            {
                this.MaxValueSlider.Value = MaximumAlcoholPercentage;
                return;
            }

            this.MaximumAlcoholContentChanged?.Invoke(this, this.MaxValueSlider.Value);
        }
    }
}