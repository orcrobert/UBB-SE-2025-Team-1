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
{   /// <summary>
    /// ViewModel for the AddDrinkMenu page. Displays a form for adding a new drink, including name, image URL, brand, alcohol content, and categories.
    /// </summary>
    /// <remarks>
    /// initializes a new instance of the <see cref="AddDrinkMenuViewModel"/> class.
    /// </remarks>
    /// <param name="drinkService">The drink service used to manage drinks.</param>
    /// <param name="userService">The user service used to manage users.</param>
    /// <param name="adminService">The admin service used to manage admin actions.</param>
    public partial class AddDrinkMenuViewModel(
        IDrinkService drinkService,
        IUserService userService,
        IAdminService adminService) : INotifyPropertyChanged
    {
        private const float MaxAlcoholContent = 100.0f;
        private const float MinAlcoholContent = 0.0f;
        private readonly IDrinkService _drinkService = drinkService;
        private readonly IUserService _userService = userService;
        private readonly IAdminService _adminService = adminService;

        /// <summary>
        /// List of all available categories.
        /// </summary>
        public List<string> AllCategories { get; set; } = new();

        /// <summary>
        /// Collection of selected categories when adding a drink.
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

        private string _newDrinkName = String.Empty;
        private string _newDrinkURL = String.Empty;
        private string _newDrinkBrandName = String.Empty;
        private string _newDrinkAlcoholContent = String.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name of the drink to be added.
        /// </summary>
        public string DrinkName
        {
            get => _newDrinkName;
            set
            {
                if (_newDrinkName != value)
                {
                    _newDrinkName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL of the image for the drink to be added.
        /// </summary>
        public string DrinkURL
        {
            get => _newDrinkURL;
            set
            {
                if (_newDrinkURL != value)
                {
                    _newDrinkURL = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the brand of the drink to be added.
        /// </summary>
        public string BrandName
        {
            get => _newDrinkBrandName;
            set
            {
                if (_newDrinkBrandName != value)
                {
                    _newDrinkBrandName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the alcohol content of the drink to be added.
        /// </summary>
        public string AlcoholContent
        {
            get => _newDrinkAlcoholContent;
            set
            {
                if (_newDrinkAlcoholContent != value)
                {
                    _newDrinkAlcoholContent = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Retrieves all the categories selected  from the list of available categories.
        /// </summary>
        public List<Category> GetSelectedCategories()
        {
            return SelectedCategoryNames
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
            if (string.IsNullOrWhiteSpace(DrinkName))
                throw new ArgumentException("Drink name is required");

            if (string.IsNullOrWhiteSpace(BrandName))
                throw new ArgumentException("Brand is required");


            if (!float.TryParse(AlcoholContent, out var alcoholContentValue) || alcoholContentValue < MinAlcoholContent || alcoholContentValue > MaxAlcoholContent)
                throw new ArgumentException("Valid alcohol content (0–100%) is required");

            if (SelectedCategoryNames.Count == 0)
                throw new ArgumentException("At least one drinkCategory must be selected");
        }


        /// <summary>
        /// Adds a new drink to the system.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the drink name, brand, or alcohol content is invalid.</exception>"
        public void InstantAddDrink()
        {
            try
            {
                //var brand = FindBrandByName(BrandName);
                var categories = GetSelectedCategories();
                float alcoholContent = float.Parse(AlcoholContent);



                _drinkService.AddDrink(
                    inputtedDrinkName: DrinkName,
                    inputtedDrinkPath: DrinkURL,
                    inputtedDrinkCategories: categories,
                    inputtedDrinkBrandName: BrandName,
                    inputtedAlcoholPercentage: alcoholContent
                );
                Debug.WriteLine("Drink added successfully (admin).");
            }
            catch (Exception DrinkValidationException)
            {
                Debug.WriteLine($"Error adding drink: {DrinkValidationException.Message}");
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
                int userId = _userService.GetCurrentUserId();
                adminService.SendNotificationFromUserToAdmin(
                    senderUserId: userId,
                    userModificationRequestType: "New Drink Request",
                    userModificationRequestDetails: $"User requested to add new drink: {DrinkName}"
                );
                Debug.WriteLine("Drink add request sent to admin.");
            }
            catch (Exception SendAddDrinkRequestException)
            {
                Debug.WriteLine($"Error sending add request: {SendAddDrinkRequestException.Message}");
                throw;
            }
        }

        /// <summary>
        /// Clears the form fields after adding a drink.
        /// </summary>
        public void ClearForm()
        {
            DrinkName = String.Empty;
            DrinkURL = String.Empty;
            BrandName = String.Empty;
            AlcoholContent = String.Empty;
            SelectedCategoryNames.Clear();
        }

        /// <summary>
        /// Triggers the PropertyChanged event for data binding.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
