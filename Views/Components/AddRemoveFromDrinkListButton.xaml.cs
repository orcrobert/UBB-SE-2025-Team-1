using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Views.ViewModels;
using System.Diagnostics;

namespace WinUIApp.Views.Components
{
    public sealed partial class AddRemoveFromDrinkListButton : UserControl
    {
        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(
                "DrinkId",
                typeof(int),
                typeof(AddRemoveFromDrinkListButton),
                new PropertyMetadata(0, OnDrinkIdChanged));

        public AddRemoveFromDrinkListButton()
        {
            this.InitializeComponent();
            Loaded += AddRemoveFromDrinkListButton_Loaded;
        }

        public int DrinkId
        {
            get { return (int)GetValue(DrinkIdProperty); }
            set { SetValue(DrinkIdProperty, value); }
        }

        private static void OnDrinkIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AddRemoveFromDrinkListButton button && (int)e.NewValue > 0)
            {
                Debug.WriteLine($"AddRemoveFromDrinkListButton: DrinkId changed to {(int)e.NewValue}");
                if (button.ViewModel == null)
                {
                    button.ViewModel = new DrinkPageViewModel((int)e.NewValue);
                    Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel created with DrinkId {(int)e.NewValue}");
                }
                else
                {
                    button.ViewModel.DrinkId = (int)e.NewValue;
                    Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel DrinkId updated to {(int)e.NewValue}");
                }
            }
        }

        public DrinkPageViewModel ViewModel
        {
            get { return (DrinkPageViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(DrinkPageViewModel),
                typeof(AddRemoveFromDrinkListButton),
                new PropertyMetadata(null, OnViewModelPropertyChanged));

        private static void OnViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AddRemoveFromDrinkListButton button && e.NewValue != null)
            {
                Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel property set");
                button.DataContext = e.NewValue;
            }
        }

        private async void AddRemoveFromDrinkListButton_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"AddRemoveFromDrinkListButton: Loaded. DrinkId: {DrinkId}, ViewModel is {(ViewModel == null ? "null" : "not null")}");
            if (ViewModel == null && DrinkId > 0)
            {
                ViewModel = new DrinkPageViewModel(DrinkId);
                Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel created in Loaded with DrinkId {DrinkId}");
            }
            if (ViewModel != null)
            {
                DataContext = ViewModel;
                await ViewModel.CheckIfInListAsync();
            }
        }

        private async void AddRemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                await ViewModel.AddRemoveFromListAsync();
            }
        }
    }
}