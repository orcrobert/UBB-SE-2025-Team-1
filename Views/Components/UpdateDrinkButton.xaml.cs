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
        private const object defaultObjectValue = null;

        public static readonly DependencyProperty DrinkProperty =
        DependencyProperty.Register(
        nameof(Drink),
        typeof(Drink),
        typeof(UpdateDrinkButton),
        new PropertyMetadata(defaultObjectValue));
        public Action OnDrinkUpdated { get; set; }

        public Drink Drink
        {
            get => (Drink)GetValue(DrinkProperty);
            set => SetValue(DrinkProperty, value);
        }

        public UpdateDrinkButton()
        {
            this.InitializeComponent();

        }

        private void UpdateDrinkButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            var userService = new WinUIApp.Services.DummyServies.UserService();
            var flyout = new Flyout
            {
                Content = new UpdateDrinkFlyout
                {
                    DrinkToUpdate = Drink,
                    UserId = userService.GetCurrentUserID()
                }
            };

            flyout.Closed += (sender, Arguments) =>
            {
                OnDrinkUpdated?.Invoke();
            };

            flyout.ShowAt(UpdateButton);

        }
    }
}
