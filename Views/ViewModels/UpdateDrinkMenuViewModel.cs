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

namespace WinUIApp.Views.ViewModels
{
    public class UpdateDrinkMenuViewModel : INotifyPropertyChanged
    {
        private readonly DrinkService _drinkService;
        private readonly UserService _userService;
        private readonly AdminService _adminService;
        public List<string> AllCategories { get; set; } = new();
        public ObservableCollection<string> SelectedCategoryNames { get; set; } = new();
        public List<Category> AllCategoryObjects { get; set; } = new();
        public List<Brand> AllBrands { get; set; } = new();

        private Drink _drinkToUpdate;

        public event PropertyChangedEventHandler PropertyChanged;

        public UpdateDrinkMenuViewModel(
            Drink drink,
            DrinkService drinkService,
            UserService userService,
            AdminService adminService)
        {
            _drinkToUpdate = drink;
            _drinkService = drinkService;
            _userService = userService;
            _adminService = adminService;
        }


        public Drink DrinkToUpdate
        {
            get => _drinkToUpdate;
            set
            {
                _drinkToUpdate = value;
                OnPropertyChanged();
            }
        }

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

        public string DrinkURL
        {
            get => DrinkToUpdate.DrinkURL;
            set
            {
                if (DrinkToUpdate.DrinkURL != value)
                {
                    DrinkToUpdate.DrinkURL = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BrandName
        {
            get => DrinkToUpdate.Brand?.Name ?? "";
            set
            {
                if (DrinkToUpdate.Brand == null || DrinkToUpdate.Brand.Name != value)
                {
                    DrinkToUpdate.Brand = new Brand(0, value);
                    OnPropertyChanged();
                }
            }
        }

        public string AlcoholContent
        {
            get => DrinkToUpdate.AlcoholContent.ToString();
            set
            {
                if (float.TryParse(value, out float parsed) && DrinkToUpdate.AlcoholContent != parsed)
                {
                    DrinkToUpdate.AlcoholContent = parsed;
                    OnPropertyChanged();
                }
            }
        }

        public List<Category> GetSelectedCategories()
        {
            return SelectedCategoryNames
                .Select(name => AllCategoryObjects.FirstOrDefault(c => c.Name == name))
                .Where(c => c != null)
                .ToList();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(DrinkName))
                throw new ArgumentException("Drink name is required");

            if (string.IsNullOrWhiteSpace(BrandName))
                throw new ArgumentException("Brand is required");

            if (string.IsNullOrWhiteSpace(BrandName))
                throw new ArgumentException("Brand is required.");

            var validBrand = AllBrands.FirstOrDefault(b => b.Name.Equals(BrandName, StringComparison.OrdinalIgnoreCase));
            if (validBrand == null)
                throw new ArgumentException("The brand you entered does not exist.");

            DrinkToUpdate.Brand = validBrand;

            if (!float.TryParse(AlcoholContent, out var alc) || alc < 0 || alc > 100)
                throw new ArgumentException("Valid alcohol content (0–100%) is required");

            if (SelectedCategoryNames.Count == 0)
                throw new ArgumentException("At least one category must be selected");
        }

        private Brand ResolveBrand(string brandName)
        {
            var existingBrands = _drinkService.getDrinkBrands();
            var match = existingBrands.FirstOrDefault(b => b.Name.Equals(brandName, StringComparison.OrdinalIgnoreCase));

            if (match == null)
                throw new ArgumentException("The brand you tried to add was not found.");

            return match;
        }

        public void InstantUpdateDrink()
        {
            try
            {
                DrinkToUpdate.Brand = ResolveBrand(BrandName);
                DrinkToUpdate.Categories = GetSelectedCategories();
                _drinkService.updateDrink(DrinkToUpdate);
                Debug.WriteLine("Drink updated successfully (admin).");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating drink: {ex.Message}");
            }
        }

        public void SendUpdateDrinkRequest()
        {
            try
            {
                int userId = _userService.GetCurrentUserID();
                _adminService.SendNotification(
                    senderUserID: userId,
                    title: "Drink Update Request",
                    description: $"User requested to update drink: {DrinkToUpdate.DrinkName}"
                );
                Debug.WriteLine("Drink update request sent to admin.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending update request: {ex.Message}");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
