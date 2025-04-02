using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Views.ViewModels;

namespace WinUIApp.Views.Components
{
    public sealed partial class AddDrinkButton : UserControl
    {
        private readonly AddDrinkMenuViewModel _viewModel;

        public AddDrinkButton()
        {
            this.InitializeComponent();
            _viewModel = new AddDrinkMenuViewModel();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveDrinkAsync();
        }
    }
}
