namespace WinUIApp.Views.Components
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.Services;

    /// <summary>
    /// A button that opens a flyout for adding a new drink.
    /// </summary>
    public sealed partial class AddDrinkButton : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddDrinkButton"/> class.
        /// </summary>
        public AddDrinkButton()
        {
            this.InitializeComponent();
        }

        private void AddDrinkButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            var userService = new WinUIApp.Services.DummyServices.UserService();
            var flyout = new Flyout
            {
                Content = new AddDrinkFlyout
                {
                    UserId = userService.GetCurrentUserId(),
                },
            };

            flyout.ShowAt(this.AddButton);
        }
    }
}