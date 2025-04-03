using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.ViewModels;
using WinUIApp.Views.Pages;

namespace WinUIApp.Views.Components
{
    public sealed partial class DrinkListComponent : UserControl
    {
        public static readonly DependencyProperty DrinksProperty =
           DependencyProperty.Register(
               "Drinks",
               typeof(List<Drink>), 
               typeof(DrinkListComponent),
               new PropertyMetadata(null));

        public List<Drink> Drinks
        {
            get => (List<Drink>)GetValue(DrinksProperty);
            set => SetValue(DrinksProperty, value);
        }

        public DrinkListComponent()
        {
            InitializeComponent();
        }

        private void DrinkItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button button && button.FindName("CardBorder") is Border border)
            {
                border.Background = (Brush)Application.Current.Resources["LayerFillColorAltBrush"];
                border.BorderBrush = (Brush)Application.Current.Resources["AccentAAFillColorTertiaryBrush"];
            }
        }

        private void DrinkItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button button && button.FindName("CardBorder") is Border border)
            {
                border.Background = (Brush)Application.Current.Resources["CardBackgroundFillColorDefaultBrush"];
                border.BorderBrush = (Brush)Application.Current.Resources["CardStrokeColorDefaultBrush"];
            }
        }

        private void DrinkItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int drinkId)
            {
                // Simple direct navigation
                MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), drinkId);
            }
        }
    }
}