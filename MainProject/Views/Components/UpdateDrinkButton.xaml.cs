// <copyright file="UpdateDrinkButton.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.UI;
    using Microsoft.UI.Text;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using WinUIApp.Models;
    using WinUIApp.Services;
    using static System.Net.WebRequestMethods;

    /// <summary>
    /// A user control that represents a button for updating a drink in the application.
    /// </summary>
    public sealed partial class UpdateDrinkButton : UserControl
    {
        /// <summary>
        /// DrinkProperty is a dependency property that represents the drink to be updated.
        /// </summary>
        public static readonly DependencyProperty DrinkProperty =
        DependencyProperty.Register(
        nameof(Drink),
        typeof(Drink),
        typeof(UpdateDrinkButton),
        new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDrinkButton"/> class.
        /// </summary>
        public UpdateDrinkButton()
        {
            this.InitializeComponent();

        }

        /// <summary>
        /// Gets or sets the drink to be updated. This property is used to bind the drink object to the button.
        /// </summary>
        public Action OnDrinkUpdated { get; set; }

        /// <summary>
        /// Gets or sets the drink to be updated. This property is used to bind the drink object to the button.
        /// </summary>
        public Drink Drink
        {
            get => (Drink)this.GetValue(DrinkProperty);
            set => this.SetValue(DrinkProperty, value);
        }

        private void UpdateDrinkButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            var userService = new WinUIApp.Services.DummyServices.UserService();
            var flyout = new Flyout
            {
                Content = new UpdateDrinkFlyout
                {
                    DrinkToUpdate = this.Drink,
                    UserId = userService.GetCurrentUserId(),
                },
            };

            flyout.Closed += (sender, arguments) =>
            {
                this.OnDrinkUpdated?.Invoke();
            };

            flyout.ShowAt(this.UpdateButton);
        }
    }
}