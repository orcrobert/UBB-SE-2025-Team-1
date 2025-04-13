// <copyright file="VoteDrinkOfTheDayButton.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components
{
    using System.Windows.Input;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.Views.ViewModels;

    /// <summary>
    /// A user control that represents a button for voting for the drink of the day.
    /// </summary>
    public sealed partial class VoteDrinkOfTheDayButton : UserControl
    {
        /// <summary>
        /// DrinkIdProperty is a dependency property that represents the ID of the drink.
        /// </summary>
        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(nameof(DrinkId), typeof(int), typeof(VoteDrinkOfTheDayButton), new PropertyMetadata(DefaultIntValue));

        private const int DefaultIntValue = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoteDrinkOfTheDayButton"/> class.
        /// </summary>
        public VoteDrinkOfTheDayButton()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the ID of the drink. This property is used to identify which drink the button is associated with.
        /// </summary>
        public int DrinkId
        {
            get => (int)this.GetValue(DrinkIdProperty);
            set => this.SetValue(DrinkIdProperty, value);
        }
    }
}