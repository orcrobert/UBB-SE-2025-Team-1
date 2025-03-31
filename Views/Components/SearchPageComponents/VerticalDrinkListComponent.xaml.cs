using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using WinUIApp.Models;

namespace WinUIApp.Views.Components
{
    public sealed partial class VerticalDrinkListComponent : UserControl
    {
        public event EventHandler<int> DrinkClicked;
        public IEnumerable<Drink> DrinksList { get; set; }

        public VerticalDrinkListComponent()
        {
            this.InitializeComponent();
        }

        private void DrinkList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Drink selectedDrink)
            {
                DrinkClicked?.Invoke(this, selectedDrink.Id);
            }
        }

        public void SetDrinks(IEnumerable<Drink> drinks)
        {
            DrinksList = drinks;
            DrinkListView.ItemsSource = drinks;
        }


    }
}
