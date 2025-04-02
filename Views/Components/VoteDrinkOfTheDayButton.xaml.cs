using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using WinUIApp.Views.ViewModels;

namespace WinUIApp.Views.Components
{
    public sealed partial class VoteDrinkOfTheDayButton : UserControl
    {
        public VoteButtonViewModel ViewModel { get; } = new VoteButtonViewModel();

        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(nameof(DrinkId), typeof(int), typeof(VoteDrinkOfTheDayButton), new PropertyMetadata(0));

        public static readonly DependencyProperty UserIdProperty =
            DependencyProperty.Register(nameof(UserId), typeof(int), typeof(VoteDrinkOfTheDayButton), new PropertyMetadata(0));

        public int DrinkId
        {
            get => (int)GetValue(DrinkIdProperty);
            set => SetValue(DrinkIdProperty, value);
        }

        public int UserId
        {
            get => (int)GetValue(UserIdProperty);
            set => SetValue(UserIdProperty, value);
        }

        public VoteDrinkOfTheDayButton()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void VoteButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.VoteForDrink(DrinkId);
        }
    }
}