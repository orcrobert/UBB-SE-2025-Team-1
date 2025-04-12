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

namespace WinUIApp.ViewModels
{
    /// <summary>
    /// ViewModel for the UpdateDrinkMenu page. Displays a form for updating an existing drinkToUpdate, including name, image URL, brand, alcohol content, and categories.
    /// </summary>
    public class UpdateDrinkMenuViewModel : INotifyPropertyChanged
    {
        private const float MaxAlcoholContent = 100.0f;
        private const float MinAlcoholContent = 0.0f;
        private readonly DrinkService _drinkService;
        private readonly UserService _userService;
        private readonly AdminService _adminService;

        /// <summary>
        /// List of all available categories.
        /// </summary>
        public List<string> AllCategories { get; set; } = new();

        /// <summary>
        /// Collection of selected categories when updating a drinkToUpdate.
        /// </summary>
        public ObservableCollection<string> SelectedCategoryNames { get; set; } = new();

        /// <summary>
        /// List of all available categories as objects. Used for data binding.
        /// </summary>
        public List<Category> AllCategoryObjects { get; set; } = new();

        /// <summary>
        /// List of all available brands.
        /// </summary>
        public List<Brand> AllBrands { get; set; } = new();

        private Drink _drinkToUpdate;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDrinkMenuViewModel"/> class.
        /// </summary>
        /// <param name="drinkToUpdate">Drink to be updated.</param>
        /// <param name="drinkService">Used to manage drinks.</param>
        /// <param name="userService">Used to manage users.</param>
        /// <param name="adminService">Used to manage admin actions.</param>
        public UpdateDrinkMenuViewModel(
            Drink drinkToUpdate,
            DrinkService drinkService,
            UserService userService,
            AdminService adminService)
        {
            _drinkToUpdate = drinkToUpdate;
            _drinkService = drinkService;
            _userService = userService;
            _adminService = adminService;
        }

        /// <summary>
        /// Gets or sets the drinkToUpdate to be updated. This property is used for data binding in the UI.
        /// </summary>
        public Drink DrinkToUpdate
        {
            get => _drinkToUpdate;
            set
            {
                _drinkToUpdate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name of the drink to be updated. This property is used for data binding in the UI. If the name changes, it raises the PropertyChanged event.
        /// </summary>
        public string DrinkName
        {
            get => DrinkToUpdate.DrinkName;
            set
            {
                if (DrinkToUpdate.DrinkName != value)
                {
                    DrinkToUpdate.DrinkName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL of the drink's image. This property is used for data binding in the UI. If the URL changes, it raises the PropertyChanged event.
        /// </summary>

        public string DrinkURL
        {
            get => DrinkToUpdate.DrinkImageUrl;
            set
            {
                if (DrinkToUpdate.DrinkImageUrl != value)
                {
                    DrinkToUpdate.DrinkImageUrl = value;
                    OnPropertyChanged();
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
                if (DrinkToUpdate.DrinkBrand == null)
                {
                    return string.Empty;
                }
                return DrinkToUpdate.DrinkBrand.BrandName;
            }
            set
            {
                if (DrinkToUpdate.DrinkBrand == null || DrinkToUpdate.DrinkBrand.BrandName != value)
                {
                    DrinkToUpdate.DrinkBrand = new Brand(0, value);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the alcohol content of the drink. This property is used for data binding in the UI. If the alcohol content changes, it raises the PropertyChanged event.
        /// </summary>
        public string AlcoholContent
        {
            get => DrinkToUpdate.AlcoholContent.ToString();
            set
            {
                if (float.TryParse(value, out float parsedAlcoholContent) && DrinkToUpdate.AlcoholContent != parsedAlcoholContent)
                {
                    DrinkToUpdate.AlcoholContent = parsedAlcoholContent;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of all available categories. This property is used for data binding in the UI. If the list changes, it raises the PropertyChanged event.
        /// </summary>
        /// <returns>The list of all available categories.</returns>
        public List<Category> GetSelectedCategories()
        {
            return SelectedCategoryNames
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
            if (string.IsNullOrWhiteSpace(DrinkName))
                throw new ArgumentException("Drink name is required");

            if (string.IsNullOrWhiteSpace(BrandName))
                throw new ArgumentException("Brand is required");

            if (string.IsNullOrWhiteSpace(BrandName))
                throw new ArgumentException("Brand is required.");

            var validBrand = AllBrands.FirstOrDefault(brand => brand.BrandName.Equals(BrandName, StringComparison.OrdinalIgnoreCase));
            if (validBrand == null)
                throw new ArgumentException("The brand you entered does not exist.");

            DrinkToUpdate.DrinkBrand = validBrand;

            if (!float.TryParse(AlcoholContent, out var alcoholContent) || alcoholContent < MinAlcoholContent || alcoholContent > MaxAlcoholContent)
                throw new ArgumentException("Valid alcohol content (0–100%) is required");

            if (SelectedCategoryNames.Count == 0)
                throw new ArgumentException("At least one category must be selected");
        }

        /// <summary>
        /// Searches for a brand by its name in the list of available brands.
        /// </summary>
        /// <param name="searchedBrandName">The name of the brand to search for.</param>
        /// <returns>The matching brand object, if found; null otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown when the brand is not found.</exception>
        private Brand FindBrandByName(string searchedBrandName)
        {
            var existingBrands = _drinkService.GetDrinkBrandNames();
            var match = existingBrands.FirstOrDefault(searchedBrand => searchedBrand.BrandName.Equals(searchedBrandName, StringComparison.OrdinalIgnoreCase));

            if (match == null)
                throw new ArgumentException("The brand you tried to add was not found.");

            return match;
        }

        /// <summary>
        /// Updates the drinkToUpdate with the new details. This method is called when the user clicks the "Update" button.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the drinkToUpdate is not found.</exception>"
        public void InstantUpdateDrink()
        {
            try
            {
                DrinkToUpdate.DrinkBrand = FindBrandByName(BrandName);
                DrinkToUpdate.CategoryList = GetSelectedCategories();
                _drinkService.UpdateDrink(DrinkToUpdate);
                Debug.WriteLine("Drink updated successfully (admin).");
            }
            catch (Exception InstantUpdateDrinkException)
            {
                Debug.WriteLine($"Error updating drinkToUpdate: {InstantUpdateDrinkException.Message}");
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
                _adminService.SendNotificationFromUserToAdmin(
                senderUserId: _userService.CurrentUserId,
                userModificationRequestType: "Drink Update Request",
                userModificationRequestDetails: $"User requested to update drinkToUpdate: {DrinkToUpdate.DrinkName}");
                Debug.WriteLine("Drink update request sent to admin.");
            }
            catch (Exception SendUpdateDrinkRequestException)
            {
                Debug.WriteLine($"Error sending update request: {SendUpdateDrinkRequestException.Message}");
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
