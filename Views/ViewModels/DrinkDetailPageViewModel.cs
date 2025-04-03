using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Models;
using WinUIApp.Services;

namespace WinUIApp.Views.ViewModels
{
    public class DrinkDetailPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DrinkService _drinkService;
        private Drink _drink;

        public Drink Drink
        {
            get { return _drink; }
            set
            {
                _drink = value;
                OnPropertyChanged(nameof(Drink));
                OnPropertyChanged(nameof(CategoriesDisplay));
            }
        }

        public string CategoriesDisplay =>
        Drink?.Categories != null
        ? string.Join(", ", Drink.Categories.Select(c => c.Name))
        : string.Empty;

        public DrinkDetailPageViewModel(DrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        public void LoadDrink(int drinkId)
        {
            Drink=_drinkService.getDrinkById(drinkId);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
