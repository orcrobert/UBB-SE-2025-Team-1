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
    /// <summary>
    /// ViewModel for the MainPage. Manages the display of the drink of the day and a personal drink list.
    /// </summary>
    class MainPageViewModel 
    {
        private const int HardCodedNumberOfDrinks = 5; // Number of drinks to display in the personal drink list.
        private DrinkService _drinkService;
        private UserService _userService;

        /// <summary>
        /// Constructor for the MainPageViewModel. Initializes the drink service and user service.
        /// </summary>
        /// <param name="drinkService"></param>
        /// <param name="userService"></param>
        public MainPageViewModel(DrinkService drinkService, UserService userService)
        {
            _drinkService = drinkService;
            _userService = userService;
            LoadDrinkOfTheDayData();
            LoadPersonalDrinkListData();
        }

        private string _imageSource;
        /// <summary>
        /// Gets or sets the image source for the drink of the day. This property is bound to the UI to display the drink's image.
        /// </summary>
        public string ImageSource
        {
            get => _imageSource;
            set => SetField(ref _imageSource, value);
        }

        private string _drinkName;

        /// <summary>
        /// Gets or sets the name of the drink of the day. This property is bound to the UI to display the drink's name.
        /// </summary>
        public string DrinkName
        {
            get => _drinkName;
            set => SetField(ref _drinkName, value);
        }

        private string _drinkBrand;

        /// <summary>
        /// Gets or sets the brand of the drink of the day. This property is bound to the UI to display the drink's brand.
        /// </summary>
        public string DrinkBrand
        {
            get => _drinkBrand;
            set => SetField(ref _drinkBrand, value);
        }

        private List<Category> _drinkCategories;

        /// <summary>
        /// Gets or sets the categories of the drink of the day. This property is bound to the UI to display the drink's categories.
        /// </summary>
        public List<Category> DrinkCategories
        {
            get => _drinkCategories;
            set => SetField(ref _drinkCategories, value);
        }

        private float _alcoholContent;

        /// <summary>
        /// Gets or sets the alcohol content of the drink of the day. This property is bound to the UI to display the drink's alcohol content.
        /// </summary>
        public float AlcoholContent
        {
            get => _alcoholContent;
            set => SetField(ref _alcoholContent, value);
        }

        /// <summary>
        /// Sets the field value and raises the PropertyChanged event if the value has changed.
        /// It is used to notify the UI about property changes.
        /// </summary>
        /// <typeparam name="TProperty">Indicates the type of the property.</typeparam>
        /// <param name="field">Refers to the field that is being set.</param>
        /// <param name="value">Refers to the new value to be set.</param>
        /// <param name="propertyName">Refers to the name of the property that is being set. This is automatically provided by the compiler.</param>
        /// <returns></returns>
        protected bool SetField<TProperty>(ref TProperty field, TProperty value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TProperty>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">Property name that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Loads the drink of the day data from the drink service. The data includes the drink's image, name, brand, categories, and alcohol content.
        /// </summary>

        public void LoadDrinkOfTheDayData()
        {
            var drink =  _drinkService.GetDrinkOfTheDay();

            ImageSource = drink.DrinkImageUrl;
            DrinkName = drink.DrinkName;
            DrinkBrand = drink.DrinkBrand.BrandName;
            DrinkCategories = drink.CategoryList;
            AlcoholContent = drink.AlcoholContent;
        }

        /// <summary>
        /// Gets the ID of the drink of the day. This ID can be used to retrieve more details about the drink.
        /// </summary>
        /// <returns>Int ID of the drink of the day.</returns>

        public int getDrinkOfTheDayId()
        {
            return _drinkService.getDrinkOfTheDay().DrinkId;
        }

        /// <summary>
        /// Gets or sets the list of personal drinks for the current user. This list is used to display the user's favorite drinks.
        /// </summary>
        public List<Drink> PersonalDrinks { get; set; } = new List<Drink>();

        /// <summary>
        /// Loads the personal drink list data for the current user. This includes retrieving the user's favorite drinks from the drink service.
        /// </summary>
        private void LoadPersonalDrinkListData()
        {
            int userId = _userService.GetCurrentUserID();
            PersonalDrinks = _drinkService.getPersonalDrinkList(userId, HardCodedNumberOfDrinks);
        }



    }
    
    
}
