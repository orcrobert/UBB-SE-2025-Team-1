using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;

namespace WinUIApp.Views.Components
{
    public sealed partial class DrinkListComponent : UserControl
    {
        public ObservableCollection<Drink> DrinksList { get; set; } = new ObservableCollection<Drink>();

        private readonly DrinkService _drinkService = new DrinkService();
        private readonly UserService _userService = new UserService();

        public DrinkListComponent()
        {
            this.InitializeComponent();
            LoadDrinks();
        }

        private void LoadDrinks()
        {
            var currentUserId = _userService.GetCurrentUserID();
            Debug.WriteLine(currentUserId);
            var drinks = _drinkService.getPersonalDrinkList(currentUserId, 5);

            Debug.WriteLine($"Retrieved {drinks.Count} drinks from the database.");
            DrinksList.Clear();
            foreach (var drink in drinks)
            {
                Debug.WriteLine($"Adding drink: {drink.DrinkName}");
                DrinksList.Add(drink);
            }
        }
    }
}