using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WinUIApp.Views.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Drink> _personalDrinks = new ObservableCollection<Drink>();
        private readonly DrinkService _drinkService = new DrinkService();
        private readonly UserService _userService = new UserService();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Drink> PersonalDrinks
        {
            get => _personalDrinks;
            set
            {
                if (_personalDrinks != value)
                {
                    _personalDrinks = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainPageViewModel()
        {
            LoadPersonalDrinks();
        }

        private async Task LoadPersonalDrinks()
        {
            var currentUserId = _userService.GetCurrentUserID();
            Debug.WriteLine($"MainPageViewModel: Current User ID: {currentUserId}");

            if (currentUserId > 0)
            {
                try
                {
                    var drinks = await Task.Run(() => _drinkService.getPersonalDrinkList(currentUserId, 5));
                    Debug.WriteLine($"MainPageViewModel: Retrieved {drinks.Count} personal drinks.");
                    PersonalDrinks.Clear();
                    foreach (var drink in drinks)
                    {
                        PersonalDrinks.Add(drink);
                        Debug.WriteLine($"MainPageViewModel: Added drink to PersonalDrinks: {drink.DrinkName}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"MainPageViewModel: Error loading personal drinks: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("MainPageViewModel: Invalid User ID, cannot load personal drinks.");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}