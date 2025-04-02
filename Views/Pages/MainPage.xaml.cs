using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Diagnostics;
using WinUIApp.ViewModels;
using WinUIApp.Views.Pages;

namespace WinUIApp.Views.Pages
{
    public sealed partial class MainPage : Page
    {
        MainPageViewModel _viewModel;
        public MainPage()
        {
            this.InitializeComponent();
            _viewModel = new MainPageViewModel();
        
        }

        private void DrinkOfTheDayComponent_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), _viewModel.getDrinkOfTheDayId());
        }



    }
}