using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class AlcoholContentFilterComponent : UserControl
    {
        public double MinimumAlcoholContent
        {
            get => MinValueSlider.Value;
            set
            {
                MinValueSlider.Value = value;
            }
        }

        private double MaximumAlcoholContent
        {
            get => MaxValueSlider.Value;
            set
            {
                MaxValueSlider.Value = value;
            }
        }

        public event EventHandler<double> MinimumAlcoholContentChanged;
        public event EventHandler<double> MaximumAlcoholContentChanged;

        public AlcoholContentFilterComponent()
        {
            this.InitializeComponent();
        }

        private void MinValueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (MinValueSlider.Value > MaxValueSlider.Value)
            {
                MinValueSlider.Value = 0;
                return;
            }
            MinimumAlcoholContentChanged?.Invoke(this, MinValueSlider.Value);
        }

        private void MaxValueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (MaxValueSlider.Value < MinValueSlider.Value)
            {
                MaxValueSlider.Value = 100;
                return;
            }
            MaximumAlcoholContentChanged?.Invoke(this, MaxValueSlider.Value);
        }

        public void ResetMinSlider()
        {
            MinValueSlider.Value = 0;
            MinimumAlcoholContentChanged?.Invoke(this, MinValueSlider.Value);
        }
        public void ResetMaxSlider()
        {
            MaxValueSlider.Value = 100;
            MaximumAlcoholContentChanged?.Invoke(this, MaxValueSlider.Value);
        }

        public void ResetSliders()
        {
            ResetMinSlider();
            ResetMaxSlider();
        }

    }
}