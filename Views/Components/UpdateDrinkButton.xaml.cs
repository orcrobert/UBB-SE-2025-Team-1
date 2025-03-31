using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinUIApp.Models;

namespace WinUIApp.Views.Components
{
    public sealed partial class UpdateDrinkButton : UserControl
    {
        public UpdateDrinkButton()
        {
            this.InitializeComponent();
        }

        private void UpdateDrinkButton_Click(object sender, RoutedEventArgs e)
        {
            var testDrink = new Drink(
                id: 1,
                categories: new List<Category>
                {
                    new Category(101, "Beer"),
                    new Category(102, "Cocktail")
                },
                brand: new Brand(201, "TestBrand"),
                alcoholContent: 5.5f
            );

            var flyout = new Flyout
            {
                Content = new UpdateDrinkFlyout
                {
                    DrinkToUpdate = testDrink
                }
            };

            flyout.ShowAt(UpdateButton);
        }
    }
}