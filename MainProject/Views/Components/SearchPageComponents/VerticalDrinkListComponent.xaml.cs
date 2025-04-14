// <copyright file="VerticalDrinkListComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using System.Collections.Generic;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// Represents a user control that displays a vertical list of drink items.
    /// </summary>
    public sealed partial class VerticalDrinkListComponent : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalDrinkListComponent"/> class.
        /// </summary>
        public VerticalDrinkListComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event that fires when a drink item is clicked, providing the ID of the selected drink.
        /// </summary>
        public event EventHandler<int> DrinkClicked;

        /// <summary>
        /// Gets or sets the collection of drink items to be displayed in the list.
        /// </summary>
        public IEnumerable<DrinkDisplayItem> DrinksList { get; set; }

        /// <summary>
        /// Updates the displayed drinks list with the provided collection of drink items.
        /// </summary>
        /// <param name="drinks">The collection of drink items to display.</param>
        public void SetDrinks(IEnumerable<DrinkDisplayItem> drinks)
        {
            this.DrinksList = drinks;
            this.DrinkListView.ItemsSource = this.DrinksList;
        }

        /// <summary>
        /// Handles click events on drink items in the list and triggers the DrinkClicked event
        /// with the ID of the selected drink.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="itemClickEventArgs">Event data containing the clicked item.</param>
        private void DrinkList_ItemClick(object sender, ItemClickEventArgs itemClickEventArgs)
        {
            if (itemClickEventArgs.ClickedItem is DrinkDisplayItem selectedDrink)
            {
                this.DrinkClicked?.Invoke(this, selectedDrink.Drink.DrinkId);
            }
        }
    }
}