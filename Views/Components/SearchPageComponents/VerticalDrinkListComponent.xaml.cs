using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class VerticalDrinkListComponent : UserControl
    {
        /// <summary>
        /// Event that fires when a drink item is clicked, providing the ID of the selected drink.
        /// </summary>
        public event EventHandler<int> DrinkClicked;

        /// <summary>
        /// Gets or sets the collection of drink items to be displayed in the list.
        /// </summary>
        public IEnumerable<DrinkDisplayItem> DrinksList { get; set; }

        /// <summary>
        /// Initializes a new instance of the VerticalDrinkListComponent control.
        /// </summary>
        public VerticalDrinkListComponent()
        {
            this.InitializeComponent();
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
                DrinkClicked?.Invoke(this, selectedDrink.Drink.Id);
            }
        }

        /// <summary>
        /// Updates the displayed drinks list with the provided collection of drink items.
        /// </summary>
        /// <param name="drinks">The collection of drink items to display.</param>
        public void SetDrinks(IEnumerable<DrinkDisplayItem> drinks)
        {
            DrinksList = drinks;
            DrinkListView.ItemsSource = DrinksList;
        }
    }


}