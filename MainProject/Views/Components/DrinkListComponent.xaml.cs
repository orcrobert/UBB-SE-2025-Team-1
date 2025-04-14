// <copyright file="DrinkListComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Media;
    using WinUIApp.Models;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;
    using WinUIApp.ViewModels;
    using WinUIApp.Views.Pages;

    /// <summary>
    /// A user control that represents a list of drinks in the application.
    /// </summary>
    public sealed partial class DrinkListComponent : UserControl
    {
        /// <summary>
        /// DrinksProperty is a dependency property that represents the list of drinks.
        /// </summary>
        public static readonly DependencyProperty DrinksProperty =
           DependencyProperty.Register(
               "Drinks",
               typeof(List<Drink>),
               typeof(DrinkListComponent),
               new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkListComponent"/> class.
        /// </summary>
        public DrinkListComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the list of drinks. This property is used to bind the list of drinks to the user control.
        /// </summary>
        public List<Drink> Drinks
        {
            get => (List<Drink>)this.GetValue(DrinksProperty);
            set => this.SetValue(DrinksProperty, value);
        }

        private void DrinkItem_Click(object sender, RoutedEventArgs eventArguments)
        {
            if (sender is Button button && button.Tag is int drinkId)
            {
                MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), drinkId);
            }
        }
    }
}