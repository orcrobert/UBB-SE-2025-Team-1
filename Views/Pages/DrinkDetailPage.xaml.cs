using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUIApp.Views.ViewModels;

namespace WinUIApp.Views.Pages
{
    public sealed partial class DrinkDetailPage : Page
    {
        public DrinkDetailPageViewModel ViewModel { get; } = new DrinkDetailPageViewModel(new Services.DrinkService(), new Services.DummyServies.ReviewService(), new Services.DummyServies.UserService(), new Services.DummyServies.AdminService());

        public DrinkDetailPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            if (ViewModel.IsCurrentUserAdmin())
                RemoveButtonText.Text = "Remove drink";
            else
                RemoveButtonText.Text = "Send remove drink request";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is int drinkId)
            {
                ViewModel.LoadDrink(drinkId);
            }
        }

        private void ConfirmRemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.RemoveDrink();
        }
    }
}