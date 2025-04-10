using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.ViewModels;
using System.Diagnostics;

namespace WinUIApp.Views.Components
{
    public sealed partial class AddRemoveFromDrinkListButton : UserControl
    {
        private const int defaultIntValue = 0;
        private const object defaultObjectValue = null;

        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(
                "DrinkId",
                typeof(int),
                typeof(AddRemoveFromDrinkListButton),
                new PropertyMetadata(defaultIntValue, OnDrinkIdChanged));

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

        private static void OnDrinkIdChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArguments)
        {
            if (dependencyObject is AddRemoveFromDrinkListButton button && (int)eventArguments.NewValue > defaultIntValue)
            {
                Debug.WriteLine($"AddRemoveFromDrinkListButton: DrinkId changed to {(int)eventArguments.NewValue}");
                if (button.ViewModel == null)
                {
                    button.ViewModel = new DrinkPageViewModel((int)eventArguments.NewValue);
                    Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel created with DrinkId {(int)eventArguments.NewValue}");
                }
                else
                {
                    button.ViewModel.DrinkId = (int)eventArguments.NewValue;
                    Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel DrinkId updated to {(int)eventArguments.NewValue}");
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
                new PropertyMetadata(defaultObjectValue, OnViewModelPropertyChanged));

        private static void OnViewModelPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArguments)
        {
            if (dependencyObject is AddRemoveFromDrinkListButton button && eventArguments.NewValue != defaultObjectValue)
            {
                Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel property set");
                button.DataContext = eventArguments.NewValue;
            }
        }

        private async void AddRemoveFromDrinkListButton_Loaded(object sender, RoutedEventArgs eventArguments)
        {
            Debug.WriteLine($"AddRemoveFromDrinkListButton: Loaded. DrinkId: {DrinkId}, ViewModel is {(ViewModel == defaultObjectValue ? "null" : "not null")}");
            if (ViewModel == defaultObjectValue && DrinkId > defaultIntValue)
            {
                ViewModel = new DrinkPageViewModel(DrinkId);
                Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel created in Loaded with DrinkId {DrinkId}");
            }
            if (ViewModel != defaultObjectValue)
            {
                DataContext = ViewModel;
                await ViewModel.CheckIfInListAsync();
            }
        }

        private async void AddRemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs eventArguments)
        {
            if (ViewModel != defaultObjectValue)
            {
                await ViewModel.AddRemoveFromListAsync();
            }
        }
    }
}