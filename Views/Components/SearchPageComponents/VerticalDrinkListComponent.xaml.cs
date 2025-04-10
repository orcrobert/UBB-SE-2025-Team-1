using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class VerticalDrinkListComponent : UserControl
    {
        public event EventHandler<int> DrinkClicked;
        public IEnumerable<DrinkDisplayItem> DrinksList { get; set; }

        public VerticalDrinkListComponent()
        {
            this.InitializeComponent();
        }

        private void DrinkList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is DrinkDisplayItem selectedDrink)
            {
                DrinkClicked?.Invoke(this, selectedDrink.Drink.DrinkId);
            }
        }

        public void SetDrinks(IEnumerable<DrinkDisplayItem> drinks)
        {
            DrinksList = drinks;
            DrinkListView.ItemsSource = DrinksList;
        }


    }
}
