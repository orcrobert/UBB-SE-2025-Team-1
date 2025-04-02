using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Diagnostics;
using System.Xml.Serialization;
using WinUIApp.ViewModels;
using WinUIApp.Views.Pages;
using WinUIApp.Services.DummyServies;
using WinUIApp.Services;
using WinUIApp.Views.Components;
namespace WinUIApp.Views.Pages
{
    public sealed partial class MainPage : Page
    {
        MainPageViewModel _viewModel;
        public MainPage()
        {
            this.InitializeComponent();
            _viewModel = new MainPageViewModel(new DrinkService(), new UserService());
            this.DataContext = _viewModel;

        }

        private void DrinkOfTheDayComponent_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), _viewModel.getDrinkOfTheDayId());
        }


    }
}