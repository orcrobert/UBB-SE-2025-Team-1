using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinUIApp.Models;
using static System.Net.WebRequestMethods;

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
                drinkName: "Ursugi",
                drinkURL: "https://static.mega-image.ro/medias/sys_master/h03/h71/9295712026654.jpg",
                categories: new List<Category>
                {
                    new Category(101, "Beer"),
                    new Category(102, "Cocktail")
                },
                brand: new Brand(201, "TestBrand"),
                alcoholContent: 5.5f
            );

            var userService = new WinUIApp.Services.DummyServies.UserService();
            var flyout = new Flyout
            {
                Content = new UpdateDrinkFlyout
                {
                    DrinkToUpdate = testDrink,
                    UserId = userService.GetCurrentUserID()
                }
            };

            flyout.ShowAt(UpdateButton);
        }
    }
}