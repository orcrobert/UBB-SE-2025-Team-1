using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUIApp.Views.ViewModels;

namespace WinUIApp.Views.Pages
{
    public sealed partial class DrinkDetailPage : Page
    {
        public DrinkDetailPageViewModel ViewModel { get; } = new DrinkDetailPageViewModel(new Services.DrinkService());

        public DrinkDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is int drinkId)
            {
                ViewModel.LoadDrink(drinkId);
            }
        }
    }
}