using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;

namespace WinUIApp.ViewModels
{
    public class AddDrinkMenuViewModel : INotifyPropertyChanged
    {
        private readonly DrinkService _drinkService;
        private readonly UserService _userService;
        private readonly AdminService _adminService;

        public List<string> AllCategories { get; set; } = new();
        public ObservableCollection<string> SelectedCategoryNames { get; set; } = new();
        public List<Category> AllCategoryObjects { get; set; } = new();
        public List<Brand> AllBrands { get; set; } = new();

        private string _drinkName = "";
        private string _drinkURL = "";
        private string _brandName = "";
        private string _alcoholContent = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public AddDrinkMenuViewModel(
            DrinkService drinkService,
            UserService userService,
            AdminService adminService)
        {
            _drinkService = drinkService;
            _userService = userService;
            _adminService = adminService;
        }

        public string DrinkName
        {
            get => _drinkName;
            set
            {
                if (_drinkName != value)
                {
                    _drinkName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DrinkURL
        {
            get => _drinkURL;
            set
            {
                if (_drinkURL != value)
                {
                    _drinkURL = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BrandName
        {
            get => _brandName;
            set
            {
                if (_brandName != value)
                {
                    _brandName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string AlcoholContent
        {
            get => _alcoholContent;
            set
            {
                if (_alcoholContent != value)
                {
                    _alcoholContent = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Category> GetSelectedCategories()
        {
            return SelectedCategoryNames
                .Select(name => AllCategoryObjects.FirstOrDefault(c => c.CategoryName == name))
                .Where(c => c != null)
                .ToList();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(DrinkName))
                throw new ArgumentException("Drink name is required");

            if (string.IsNullOrWhiteSpace(BrandName))
                throw new ArgumentException("Brand is required");


            if (!float.TryParse(AlcoholContent, out var alc) || alc < 0 || alc > 100)
                throw new ArgumentException("Valid alcohol content (0–100%) is required");

            if (SelectedCategoryNames.Count == 0)
                throw new ArgumentException("At least one category must be selected");
        }

        private Brand ResolveBrand(string brandName)
        {
            var existingBrands = _drinkService.getDrinkBrands();
            var match = existingBrands.FirstOrDefault(b => b.BrandName.Equals(brandName, StringComparison.OrdinalIgnoreCase));

            if (match == null)
                throw new ArgumentException("The brand you tried to add was not found.");

            return match;
        }

        public void InstantAddDrink()
        {
            try
            {
                //var brand = ResolveBrand(BrandName);
                var categories = GetSelectedCategories();
                float alcoholContent = float.Parse(AlcoholContent);

                _drinkService.addDrink(
                    drinkName: DrinkName,
                    drinkUrl: DrinkURL,
                    categories: categories,
                    brandName: BrandName,
                    alcoholContent: alcoholContent
                );
                Debug.WriteLine("Drink added successfully (admin).");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding drink: {ex.Message}");
                throw;
            }
        }

        public void SendAddDrinkRequest()
        {
            try
            {
                int userId = _userService.GetCurrentUserID();
                _adminService.SendNotification(
                    senderUserID: userId,
                    title: "New Drink Request",
                    description: $"User requested to add new drink: {DrinkName}"
                );
                Debug.WriteLine("Drink add request sent to admin.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending add request: {ex.Message}");
                throw;
            }
        }

        public void ClearForm()
        {
            DrinkName = "";
            DrinkURL = "";
            BrandName = "";
            AlcoholContent = "";
            SelectedCategoryNames.Clear();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
