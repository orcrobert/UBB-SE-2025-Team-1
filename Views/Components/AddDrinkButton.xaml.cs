using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using WinUIApp.Services;
using WinUIApp.Views.ViewModels;

namespace WinUIApp.Views.Components
{
    public sealed partial class AddDrinkButton : UserControl
    {
        public AddDrinkButton()
        {
            this.InitializeComponent();
            this.DataContext = new AddDrinkMenuViewModel();
        }

        private void AddDrinkButton_Click(object sender, RoutedEventArgs e)
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