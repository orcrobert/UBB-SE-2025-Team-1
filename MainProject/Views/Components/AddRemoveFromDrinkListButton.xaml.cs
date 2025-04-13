// <copyright file="AddRemoveFromDrinkListButton.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using System.Diagnostics;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;
    using WinUIApp.ViewModels;

    /// <summary>
    /// A button that allows users to add or remove a drink from their personal drink list.
    /// </summary>
    public sealed partial class AddRemoveFromDrinkListButton : UserControl
    {
        private readonly IDrinkService _drinkService;
        private readonly IUserService _userService;


        public AddRemoveFromDrinkListButton()
        {
            _drinkService = new DrinkService();
            _userService = new UserService();
            this.InitializeComponent();
            this.Loaded += this.AddRemoveFromDrinkListButton_Loaded;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddRemoveFromDrinkListButton"/> class.
        /// </summary>
        public AddRemoveFromDrinkListButton(IDrinkService drinkService, IUserService userService)
        {
            this.InitializeComponent();
            _drinkService = drinkService;
            _userService = userService;
            this.Loaded += this.AddRemoveFromDrinkListButton_Loaded;

        }
        /// <summary>
        /// DrinkIdProperty is a dependency property that represents the ID of the drink.
        /// </summary>
        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(
                "DrinkId",
                typeof(int),
                typeof(AddRemoveFromDrinkListButton),
                new PropertyMetadata(DefaultIntValue, new PropertyChangedCallback(OnDrinkIdChanged)));

        /// <summary>
        /// ViewModelProperty is a dependency property that represents the ViewModel for the button.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(DrinkPageViewModel),
                typeof(AddRemoveFromDrinkListButton),
                new PropertyMetadata(null, OnViewModelPropertyChanged));

        private const int DefaultIntValue = 0;


        /// <summary>
        /// Gets or sets the ID of the drink. This property is used to identify which drink the button is associated with.
        /// </summary>
        public int DrinkId
        {
            get { return (int)this.GetValue(DrinkIdProperty); }
            set { this.SetValue(DrinkIdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ViewModel for the button. This property is used to bind the button to the ViewModel that manages the drink's state.
        /// </summary>
        public DrinkPageViewModel ViewModel
        {
            get { return (DrinkPageViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        private static void OnDrinkIdChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArguments)
        {
            if (dependencyObject is AddRemoveFromDrinkListButton button && (int)eventArguments.NewValue > DefaultIntValue)
            {
                Debug.WriteLine($"AddRemoveFromDrinkListButton: DrinkId changed to {(int)eventArguments.NewValue}");
                if (button.ViewModel == null)
                {
                    button.ViewModel = new DrinkPageViewModel((int)eventArguments.NewValue, button._drinkService, button._userService);
                    Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel created with DrinkId {(int)eventArguments.NewValue}");
                }
                else
                {
                    button.ViewModel.DrinkId = (int)eventArguments.NewValue;
                    Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel DrinkId updated to {(int)eventArguments.NewValue}");
                }
            }
        }

        private static void OnViewModelPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArguments)
        {
            if (dependencyObject is AddRemoveFromDrinkListButton button && eventArguments.NewValue != null)
            {
                Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel property set");
                button.DataContext = eventArguments.NewValue;
            }
        }

        private async void AddRemoveFromDrinkListButton_Loaded(object sender, RoutedEventArgs eventArguments)
        {
            Debug.WriteLine($"AddRemoveFromDrinkListButton: Loaded. DrinkId: {this.DrinkId}, ViewModel is {(this.ViewModel == null ? "null" : "not null")}");
            if (this.ViewModel == null && this.DrinkId > DefaultIntValue)
            {
                this.ViewModel = new DrinkPageViewModel(this.DrinkId,_drinkService,this._userService);
                Debug.WriteLine($"AddRemoveFromDrinkListButton: ViewModel created in Loaded with DrinkId {this.DrinkId}");
            }

            if (this.ViewModel != null)
            {
                this.DataContext = this.ViewModel;
                await this.ViewModel.CheckIfInListAsync();
            }
        }

        private async void AddRemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs eventArguments)
        {
            if (this.ViewModel != null)
            {
                await this.ViewModel.AddRemoveFromListAsync();
            }
        }
    }
}