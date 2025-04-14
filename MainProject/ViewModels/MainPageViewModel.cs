// <copyright file="MainPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using WinUIApp.Models;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;

    /// <summary>
    /// ViewModel for the MainPage. Manages the display of the drink of the day and a personal drink list.
    /// </summary>
    public class MainPageViewModel
    {
        private const int HardCodedNumberOfDrinks = 5;
        private IDrinkService drinkService;
        private IUserService userService;
        private string imageSource;
        private string drinkName;
        private string drinkBrand;
        private List<Category> drinkCategories;
        private float alcoholContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        /// <param name="drinkService"> Drink service. </param>
        /// <param name="userService"> User service. </param>
        public MainPageViewModel(IDrinkService drinkService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
            this.LoadDrinkOfTheDayData();
            this.LoadPersonalDrinkListData();
        }

        /// <summary>
        /// Event handler for property changes. This is used for data binding in the UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the image source for the drink of the day. This property is bound to the UI to display the drink's image.
        /// </summary>
        public string ImageSource
        {
            get => this.imageSource;
            set => this.SetField(ref this.imageSource, value);
        }

        /// <summary>
        /// Gets or sets the name of the drink of the day. This property is bound to the UI to display the drink's name.
        /// </summary>
        public string DrinkName
        {
            get => this.drinkName;
            set => this.SetField(ref this.drinkName, value);
        }

        /// <summary>
        /// Gets or sets the brand of the drink of the day. This property is bound to the UI to display the drink's brand.
        /// </summary>
        public string DrinkBrand
        {
            get => this.drinkBrand;
            set => this.SetField(ref this.drinkBrand, value);
        }

        /// <summary>
        /// Gets or sets the categories of the drink of the day. This property is bound to the UI to display the drink's categories.
        /// </summary>
        public List<Category> DrinkCategories
        {
            get => this.drinkCategories;
            set => this.SetField(ref this.drinkCategories, value);
        }

        /// <summary>
        /// Gets or sets the alcohol content of the drink of the day. This property is bound to the UI to display the drink's alcohol content.
        /// </summary>
        public float AlcoholContent
        {
            get => this.alcoholContent;
            set => this.SetField(ref this.alcoholContent, value);
        }

        /// <summary>
        /// Gets or sets the list of personal drinks for the current user. This list is used to display the user's favorite drinks.
        /// </summary>
        public List<Drink> PersonalDrinks { get; set; } = new List<Drink>();

        /// <summary>
        /// Loads the drink of the day data from the drink service. The data includes the drink's image, name, brand, categories, and alcohol content.
        /// </summary>
        public void LoadDrinkOfTheDayData()
        {
            var drink = this.drinkService.GetDrinkOfTheDay();

            this.ImageSource = drink.DrinkImageUrl;
            this.DrinkName = drink.DrinkName;
            this.DrinkBrand = drink.DrinkBrand.BrandName;
            this.DrinkCategories = drink.CategoryList;
            this.AlcoholContent = drink.AlcoholContent;
        }

        /// <summary>
        /// Gets the ID of the drink of the day. This ID can be used to retrieve more details about the drink.
        /// </summary>
        /// <returns>Int ID of the drink of the day.</returns>
        public int GetDrinkOfTheDayId()
        {
            return this.drinkService.GetDrinkOfTheDay().DrinkId;
        }

        /// <summary>
        /// Sets the field value and raises the PropertyChanged event if the value has changed.
        /// It is used to notify the UI about property changes.
        /// </summary>
        /// <typeparam name="TProperty">Indicates the type of the property.</typeparam>
        /// <param name="field">Refers to the field that is being set.</param>
        /// <param name="value">Refers to the new value to be set.</param>
        /// <param name="propertyName">Refers to the name of the property that is being set. This is automatically provided by the compiler.</param>
        /// <returns>Bool.</returns>
        protected bool SetField<TProperty>(ref TProperty field, TProperty value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TProperty>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">Property name that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Loads the personal drink list data for the current user. This includes retrieving the user's favorite drinks from the drink service.
        /// </summary>
        private void LoadPersonalDrinkListData()
        {
            int userId = this.userService.GetCurrentUserId();
            this.PersonalDrinks = this.drinkService.GetUserPersonalDrinkList(userId, HardCodedNumberOfDrinks);
        }
    }
}