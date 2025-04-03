using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using WinUIApp.Views.ViewModels;

namespace WinUIApp.Views.Components
{
    public sealed partial class VoteDrinkOfTheDayButton : UserControl
    {

        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(nameof(DrinkId), typeof(int), typeof(VoteDrinkOfTheDayButton), new PropertyMetadata(0));

        public int DrinkId
        {
            get => (int)GetValue(DrinkIdProperty);
            set => SetValue(DrinkIdProperty, value);
        }

        public VoteDrinkOfTheDayButton()
        {
            InitializeComponent();
        }

        
    }
}