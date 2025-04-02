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
            var service = new WinUIApp.Services.DrinkService();
            var testDrink = service.getDrinks(null, null, null, null, null, null)[0];

            Debug.WriteLine($"Name: {testDrink.DrinkName}");
            Debug.WriteLine($"Brand: {testDrink.Brand}");
            Debug.WriteLine($"Alcohol: {testDrink.AlcoholContent}%");
            Debug.WriteLine("Categories: " + string.Join(", ", testDrink.Categories.Select(c => c.Name)));

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