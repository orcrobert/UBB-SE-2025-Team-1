// <copyright file="UpdateDrinkMenuViewModel.cs" company="PlaceholderCompany">
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
    /// ViewModel for the UpdateDrinkMenu page. Displays a form for updating an existing drinkToUpdate, including name, image URL, brand, alcohol content, and categories.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UpdateDrinkMenuViewModel"/> class.
    /// </remarks>
    /// <param name="drinkToUpdate">Drink to be updated.</param>
    /// <param name="drinkService">Used to manage drinks.</param>
    /// <param name="userService">Used to manage users.</param>
    /// <param name="adminService">Used to manage admin actions.</param>
    public partial class UpdateDrinkMenuViewModel(
        Drink drinkToUpdate,
        IDrinkService drinkService,
        IUserService userService,
        IAdminService adminService) : INotifyPropertyChanged
    {
        private const float MaxAlcoholContent = 100.0f;
        private const float MinAlcoholContent = 0.0f;
        private readonly IDrinkService drinkService = drinkService;
        private readonly IUserService userService = userService;
        private readonly IAdminService adminService = adminService;
        private Drink drinkToUpdate = drinkToUpdate;

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
        /// Gets or sets the drinkToUpdate to be updated. This property is used for data binding in the UI.
        /// </summary>
        public Drink DrinkToUpdate
        {
            get => this.drinkToUpdate;
            set
            {
                this.drinkToUpdate = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name of the drink to be updated. This property is used for data binding in the UI. If the name changes, it raises the PropertyChanged event.
        /// </summary>
        public string DrinkName
        {
            get => this.DrinkToUpdate.DrinkName;
            set
            {
                if (this.DrinkToUpdate.DrinkName != value)
                {
                    this.DrinkToUpdate.DrinkName = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL of the drink's image. This property is used for data binding in the UI. If the URL changes, it raises the PropertyChanged event.
        /// </summary>

        public string DrinkURL
        {
            get => this.DrinkToUpdate.DrinkImageUrl;
            set
            {
                if (this.DrinkToUpdate.DrinkImageUrl != value)
                {
                    this.DrinkToUpdate.DrinkImageUrl = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the brand of the drink. This property is used for data binding in the UI. If the brand name changes, it raises the PropertyChanged event.
        /// If the brand name is null, it returns an empty string.
        /// </summary>
        public string BrandName
        {
            get
            {
                if (this.DrinkToUpdate.DrinkBrand == null)
                {
                    return string.Empty;
                }

                return this.DrinkToUpdate.DrinkBrand.BrandName;
            }

            set
            {
                if (this.DrinkToUpdate.DrinkBrand == null || this.DrinkToUpdate.DrinkBrand.BrandName != value)
                {
                    this.DrinkToUpdate.DrinkBrand = new Brand(0, value);
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the alcohol content of the drink. This property is used for data binding in the UI. If the alcohol content changes, it raises the PropertyChanged event.
        /// </summary>
        public string AlcoholContent
        {
            get => this.DrinkToUpdate.AlcoholContent.ToString();
            set
            {
                if (float.TryParse(value, out float parsedAlcoholContent) && this.DrinkToUpdate.AlcoholContent != parsedAlcoholContent)
                {
                    this.DrinkToUpdate.AlcoholContent = parsedAlcoholContent;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of all available categories. This property is used for data binding in the UI. If the list changes, it raises the PropertyChanged event.
        /// </summary>
        /// <returns>The list of all available categories.</returns>
        public List<Category> GetSelectedCategories()
        {
            return this.SelectedCategoryNames
                .Select(name => AllCategoryObjects.FirstOrDefault(category => category.CategoryName == name))
                .Where(SelectedCategory => SelectedCategory != null)
                .ToList();
        }

        /// <summary>
        /// Validates the updated drink details. This method checks if the drink name, brand, alcohol content, and categories are valid.
        /// The drink name and brand are required, the alcohol content must be a valid float between 0 and 100, and at least one category must be selected.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when any of the drink details are invalid.</exception>
        public void ValidateUpdatedDrinkDetails()
        {
            if (string.IsNullOrWhiteSpace(this.DrinkName))
            {
                throw new ArgumentException("Drink name is required");
            }

            if (string.IsNullOrWhiteSpace(this.BrandName))
            {
                throw new ArgumentException("Brand is required.");
            }

            var validBrand = this.AllBrands.FirstOrDefault(brand => brand.BrandName.Equals(this.BrandName, StringComparison.OrdinalIgnoreCase));
            if (validBrand == null)
            {
                throw new ArgumentException("The brand you entered does not exist.");
            }

            this.DrinkToUpdate.DrinkBrand = validBrand;

            if (!float.TryParse(this.AlcoholContent, out var alcoholContent) || alcoholContent < MinAlcoholContent || alcoholContent > MaxAlcoholContent)
            {
                throw new ArgumentException("Valid alcohol content (0–100%) is required");
            }

            if (this.SelectedCategoryNames.Count == 0)
            {
                throw new ArgumentException("At least one category must be selected");
            }
        }

        /// <summary>
        /// Updates the drinkToUpdate with the new details. This method is called when the user clicks the "Update" button.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the drinkToUpdate is not found.</exception>"
        public void InstantUpdateDrink()
        {
            try
            {
                this.DrinkToUpdate.DrinkBrand = this.FindBrandByName(this.BrandName);
                this.DrinkToUpdate.CategoryList = this.GetSelectedCategories();
                this.drinkService.UpdateDrink(this.DrinkToUpdate);
                Debug.WriteLine("Drink updated successfully (admin).");
            }
            catch (Exception instantUpdateDrinkException)
            {
                Debug.WriteLine($"Error updating drinkToUpdate: {instantUpdateDrinkException.Message}");
            }
        }

        /// <summary>
        /// Sends a notification to the admin requesting an update for the drinkToUpdate.
        /// </summary>
        /// <exception cref="Exception">Thrown when there is an error sending the notification.</exception>"
        public void SendUpdateDrinkRequest()
        {
            try
            {
                this.adminService.SendNotificationFromUserToAdmin(
                senderUserId: this.userService.CurrentUserId,
                userModificationRequestType: "Drink Update Request",
                userModificationRequestDetails: $"User requested to update drinkToUpdate: {this.DrinkToUpdate.DrinkName}");
                Debug.WriteLine("Drink update request sent to admin.");
            }
            catch (Exception sendUpdateDrinkRequestException)
            {
                Debug.WriteLine($"Error sending update request: {sendUpdateDrinkRequestException.Message}");
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Searches for a brand by its name in the list of available brands.
        /// </summary>
        /// <param name="searchedBrandName">The name of the brand to search for.</param>
        /// <returns>The matching brand object, if found; null otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown when the brand is not found.</exception>
        private Brand FindBrandByName(string searchedBrandName)
        {
            var existingBrands = this.drinkService.GetDrinkBrandNames();
            var match = existingBrands.FirstOrDefault(searchedBrand => searchedBrand.BrandName.Equals(searchedBrandName, StringComparison.OrdinalIgnoreCase));

            if (match == null)
            {
                throw new ArgumentException("The brand you tried to add was not found.");
            }

            return match;
        }
    }
}