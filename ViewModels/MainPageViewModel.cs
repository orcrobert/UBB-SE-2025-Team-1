using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;

namespace WinUIApp.ViewModels
{
    class MainPageViewModel 
    {
        private DrinkService _drinkService;
        private UserService _userService;

        public MainPageViewModel()
        {
            _drinkService = new DrinkService();
            _userService = new UserService();
            LoadDrinkOfTheDayData();
            LoadPersonalDrinkListData();
        }

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set => SetField(ref _imageSource, value);
        }

        private string _drinkName;
        public string DrinkName
        {
            get => _drinkName;
            set => SetField(ref _drinkName, value);
        }

        private string _drinkBrand;
        public string DrinkBrand
        {
            get => _drinkBrand;
            set => SetField(ref _drinkBrand, value);
        }

        private List<Category> _drinkCategories;
        public List<Category> DrinkCategories
        {
            get => _drinkCategories;
            set => SetField(ref _drinkCategories, value);
        }

        private float _alcoholContent;
        public float AlcoholContent
        {
            get => _alcoholContent;
            set => SetField(ref _alcoholContent, value);
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadDrinkOfTheDayData()
        {
            var drink =  _drinkService.getDrinkOfTheDay();

            ImageSource = drink.DrinkURL;
            DrinkName = drink.DrinkName;
            DrinkBrand = drink.Brand.Name;
            DrinkCategories = drink.Categories;
            AlcoholContent = drink.AlcoholContent;
        }

        public int getFrinkOfTheDayId()
        {
            return _drinkService.getDrinkOfTheDay().Id;
        }

        public List<Drink> PersonalDrinks { get; set; } = new List<Drink>();

        private void LoadPersonalDrinkListData()
        {
            int userId = _userService.GetCurrentUserID();
            PersonalDrinks = _drinkService.getPersonalDrinkList(userId, 5);
        }

    }
    
    
}
