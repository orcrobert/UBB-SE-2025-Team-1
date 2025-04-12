using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using WinUIApp.Services;
using WinUIApp.Services.DummyServices;
using WinUIApp.ViewModels;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainWindow.PreviousPage = typeof(MainPage);
        }

    }
}