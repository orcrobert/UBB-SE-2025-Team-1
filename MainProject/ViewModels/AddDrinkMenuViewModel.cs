// <copyright file="AddDrinkMenuViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using WinUIApp.Models;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;

    /// <summary>
    /// ViewModel for the AddDrinkMenu page. Displays a form for adding a new drink, including name, image URL, brand, alcohol content, and categories.
    /// </summary>
    /// <remarks>
    /// initializes a new instance of the <see cref="AddDrinkMenuViewModel"/> class.
    /// </remarks>
    /// <param name="drinkService">The drink service used to manage drinks.</param>
    /// <param name="userService">The user service used to manage users.</param>
    /// <param name="adminService">The admin service used to manage admin actions.</param>
    public partial class AddDrinkMenuViewModel(
        DrinkService drinkService,
        UserService userService,
        AdminService adminService) : INotifyPropertyChanged
    {
        private const float MaxAlcoholContent = 100.0f;
        private const float MinAlcoholContent = 0.0f;
        private readonly DrinkService drinkService = drinkService;
        private readonly UserService userService = userService;
        private readonly AdminService adminService = adminService;
        private string newDrinkName = string.Empty;
        private string newDrinkURL = string.Empty;
        private string newDrinkBrandName = string.Empty;
        private string newDrinkAlcoholContent = string.Empty;

        /// <summary>
        /// Event handler for property changes. This is used for data binding in the UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the list of all available drink categories.
        /// </summary>
        public List<string> AllCategories { get; set; } = new ();

        /// <summary>
        /// Gets or sets the list of selected drink categories.
        /// </summary>
        public ObservableCollection<string> SelectedCategoryNames { get; set; } = new ();

        /// <summary>
        /// Gets or sets the list of all available drink categories as objects.
        /// </summary>
        public List<Category> AllCategoryObjects { get; set; } = new ();

        /// <summary>
        /// Gets or sets the list of all available drink brands.
        /// </summary>
        public List<Brand> AllBrands { get; set; } = new ();

        /// <summary>
        /// Gets or sets the name of the drink to be added.
        /// </summary>
        public string DrinkName
        {
            get => this.newDrinkName;
            set
            {
                if (this.newDrinkName != value)
                {
                    this.newDrinkName = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL of the image for the drink to be added.
        /// </summary>
        public string DrinkURL
        {
            get => this.newDrinkURL;
            set
            {
                if (this.newDrinkURL != value)
                {
                    this.newDrinkURL = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the brand of the drink to be added.
        /// </summary>
        public string BrandName
        {
            get => this.newDrinkBrandName;
            set
            {
                if (this.newDrinkBrandName != value)
                {
                    this.newDrinkBrandName = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the alcohol content of the drink to be added.
        /// </summary>
        public string AlcoholContent
        {
            get => this.newDrinkAlcoholContent;
            set
            {
                if (this.newDrinkAlcoholContent != value)
                {
                    this.newDrinkAlcoholContent = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Retrieves the list of all available drink categories from the drink service.
        /// </summary>
        /// <returns> List. </returns>
        public List<Category> GetSelectedCategories()
        {
            return this.SelectedCategoryNames
                .Select(name => AllCategoryObjects.FirstOrDefault(drinkCategory => drinkCategory.CategoryName == name))
                .Where(selectedCategory => selectedCategory != null)
                .ToList();
        }

        /// <summary>
        /// Validates the input data for adding a drink.
        /// The drink name must not be empty, the brand must exist, the alcohol content must be a valid number between 0 and 100, and at least one drinkCategory must be selected.
        /// </summary>
        public void ValidateUserDrinkInput()
        {
            if (string.IsNullOrWhiteSpace(this.DrinkName))
            {
                throw new ArgumentException("Drink name is required");
            }

            if (string.IsNullOrWhiteSpace(this.BrandName))
            {
                throw new ArgumentException("Brand is required");
            }

            if (!float.TryParse(this.AlcoholContent, out var alcoholContentValue) || alcoholContentValue < MinAlcoholContent || alcoholContentValue > MaxAlcoholContent)
            {
                throw new ArgumentException("Valid alcohol content (0–100%) is required");
            }

            if (this.SelectedCategoryNames.Count == 0)
            {
                throw new ArgumentException("At least one drinkCategory must be selected");
            }
        }

        /// <summary>
        /// Adds a new drink to the system.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the drink name, brand, or alcohol content is invalid.</exception>"
        public void InstantAddDrink()
        {
            try
            {
                var categories = this.GetSelectedCategories();
                float alcoholContent = float.Parse(this.AlcoholContent);

                this.drinkService.AddDrink(
                    inputtedDrinkName: this.DrinkName,
                    inputtedDrinkPath: this.DrinkURL,
                    inputtedDrinkCategories: categories,
                    inputtedDrinkBrandName: this.BrandName,
                    inputtedAlcoholPercentage: alcoholContent);
                Debug.WriteLine("Drink added successfully (admin).");
            }
            catch (Exception drinkValidationException)
            {
                Debug.WriteLine($"Error adding drink: {drinkValidationException.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sends a request to the admin to add a new drink.
        /// </summary>
        /// <exception cref="Exception">Thrown when there is an error sending the request.</exception>"
        public void SendAddDrinkRequest()
        {
            try
            {
                int userId = this.userService.GetCurrentUserId();
                this.adminService.SendNotificationFromUserToAdmin(
                    senderUserId: userId,
                    userModificationRequestType: "New Drink Request",
                    userModificationRequestDetails: $"User requested to add new drink: {this.DrinkName}");
                Debug.WriteLine("Drink add request sent to admin.");
            }
            catch (Exception sendAddDrinkRequestException)
            {
                Debug.WriteLine($"Error sending add request: {sendAddDrinkRequestException.Message}");
                throw;
            }
        }

        /// <summary>
        /// Clears the form fields after adding a drink.
        /// </summary>
        public void ClearForm()
        {
            this.DrinkName = string.Empty;
            this.DrinkURL = string.Empty;
            this.BrandName = string.Empty;
            this.AlcoholContent = string.Empty;
            this.SelectedCategoryNames.Clear();
        }

        /// <summary>
        /// Notifies the UI that a property has changed.
        /// </summary>
        /// <param name="propertyName"> Name. </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}