using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using WinUIApp.Services; // Assuming DrinkService exists here

namespace WinUIApp.Views.Components
{
    public sealed partial class AddRemoveFromDrinkListButton : UserControl
    {
        public static readonly DependencyProperty UserIdProperty =
            DependencyProperty.Register(
                "UserId",
                typeof(int),
                typeof(AddRemoveFromDrinkListButton),
                new PropertyMetadata(0, OnUserIdChanged));

        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(
                "DrinkId",
                typeof(int),
                typeof(AddRemoveFromDrinkListButton),
                new PropertyMetadata(0, OnDrinkIdChanged));

        private bool _isInList = false;
        private readonly DrinkService _drinkService;

        public AddRemoveFromDrinkListButton()
        {
            this.InitializeComponent();
            _drinkService = new DrinkService();
            this.Loaded += AddRemoveFromDrinkListButton_Loaded;
        }

        public int UserId
        {
            get { return (int)GetValue(UserIdProperty); }
            set { SetValue(UserIdProperty, value); }
        }

        public int DrinkId
        {
            get { return (int)GetValue(DrinkIdProperty); }
            set { SetValue(DrinkIdProperty, value); }
        }

        private static void OnUserIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AddRemoveFromDrinkListButton button)
            {
                if (button.DrinkId > 0 && (int)e.NewValue > 0)
                {
                    button.CheckIfInList();
                }
            }
        }

        private static void OnDrinkIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AddRemoveFromDrinkListButton button)
            {
                if ((int)e.NewValue > 0 && button.UserId > 0)
                {
                    button.CheckIfInList();
                }
            }
        }

        private void AddRemoveFromDrinkListButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserId > 0 && DrinkId > 0)
            {
                 CheckIfInList();
            }
        }

        private void CheckIfInList()
        {
            try
            {
                _isInList = _drinkService.isDrinkInPersonalList(UserId, DrinkId);
                UpdateButtonStyle();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking drink list: {ex.Message}");
            }
        }

        private void AddRemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (UserId <= 0 || DrinkId <= 0)
            {
                return;
            }

            try
            {
                if (_isInList)
                {
                    bool success = _drinkService.deleteFromPersonalDrinkList(UserId, DrinkId);
                    if (success)
                    {
                        _isInList = false;
                        UpdateButtonStyle();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to remove drink from personal list.");
                    }
                }
                else
                {
                    bool success = _drinkService.addToPersonalDrinkList(UserId, DrinkId);
                    if (success)
                    {
                        _isInList = true;
                        UpdateButtonStyle();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to add drink to personal list.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating drink list: {ex.Message}");
            }
        }

        private void UpdateButtonStyle()
        {
            if (_isInList)
            {
                ButtonText.Text = "\u2665";
            }
            else
            {
                ButtonText.Text = "\U0001F5A4";
            }
            ButtonText.FontFamily = new Microsoft.UI.Xaml.Media.FontFamily("Segoe UI Emoji");
        }
    }
}