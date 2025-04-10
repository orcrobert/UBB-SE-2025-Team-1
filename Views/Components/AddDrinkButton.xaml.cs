using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Services;

namespace WinUIApp.Views.Components
{
    public sealed partial class AddDrinkButton : UserControl
    {
        public AddDrinkButton()
        {
            this.InitializeComponent();
        }

        private void AddDrinkButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            var userService = new WinUIApp.Services.DummyServies.UserService();
            var flyout = new Flyout
            {
                Content = new AddDrinkFlyout
                {
                    UserId = userService.GetCurrentUserID()
                }
            };

            flyout.ShowAt(AddButton);
        }
    }
} 