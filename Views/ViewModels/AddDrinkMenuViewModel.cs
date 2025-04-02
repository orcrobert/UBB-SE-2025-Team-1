using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;

namespace WinUIApp.Views.ViewModels
{
    public class AddDrinkMenuViewModel : INotifyPropertyChanged
    {
        private readonly DrinkService _drinkService;
        private readonly AdminService _adminService;
        private readonly UserService _userService;
        private int _userId;
        private string _drinkName;
        private string _imageUrl;
        private string _brandName;
        private float _alcoholContent;
        private List<string> _allCategories;
        private HashSet<string> _selectedCategoryNames;
        private string _searchQuery;
        private bool _isAdmin;
        private string _buttonText;

        public event PropertyChangedEventHandler PropertyChanged;

        public AddDrinkMenuViewModel()
        {
            _drinkService = new DrinkService();
            _adminService = new AdminService();
            _userService = new UserService();
            _allCategories = new List<string>();
            _selectedCategoryNames = new HashSet<string>();
            ButtonText = "Add Drink";
            LoadCategories();
        }

        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                    IsAdmin = _adminService.IsAdmin(_userId);
                    ButtonText = IsAdmin ? "Add Drink" : "Send Request to Admin";
                }
            }
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

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
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

        public float AlcoholContent
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

        public List<string> AllCategories
        {
            get => _allCategories;
            set
            {
                if (_allCategories != value)
                {
                    _allCategories = value;
                    OnPropertyChanged();
                }
            }
        }

        public HashSet<string> SelectedCategoryNames
        {
            get => _selectedCategoryNames;
            set
            {
                if (_selectedCategoryNames != value)
                {
                    _selectedCategoryNames = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    FilterCategories();
                }
            }
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    OnPropertyChanged();
                    ButtonText = value ? "Add Drink" : "Send Request to Admin";
                }
            }
        }

        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (_buttonText != value)
                {
                    _buttonText = value;
                    OnPropertyChanged();
                }
            }
        }

        private void LoadCategories()
        {
            var categories = _drinkService.getDrinkCategories();
            AllCategories = categories.Select(c => c.Name).ToList();
        }

        private void FilterCategories()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                LoadCategories();
                return;
            }

            var filtered = AllCategories
                .Where(c => c.ToLower().Contains(SearchQuery.ToLower()))
                .ToList();
            AllCategories = filtered;
        }

        public void AddCategory(string categoryName)
        {
            if (!SelectedCategoryNames.Contains(categoryName))
            {
                SelectedCategoryNames.Add(categoryName);
                OnPropertyChanged(nameof(SelectedCategoryNames));
            }
        }

        public void RemoveCategory(string categoryName)
        {
            if (SelectedCategoryNames.Contains(categoryName))
            {
                SelectedCategoryNames.Remove(categoryName);
                OnPropertyChanged(nameof(SelectedCategoryNames));
            }
        }

        public async Task SaveDrinkAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DrinkName))
                    throw new ArgumentException("Drink name is required");

                if (string.IsNullOrWhiteSpace(BrandName))
                    throw new ArgumentException("Brand is required");

                if (AlcoholContent < 0 || AlcoholContent > 100)
                    throw new ArgumentException("Valid alcohol content (0-100%) is required");

                if (SelectedCategoryNames.Count == 0)
                    throw new ArgumentException("At least one category must be selected");

                var categories = _drinkService.getDrinkCategories()
                    .Where(c => SelectedCategoryNames.Contains(c.Name))
                    .ToList();

                if (IsAdmin)
                {
                    await Task.Run(() => _drinkService.addDrink(
                        DrinkName,
                        ImageUrl,
                        categories,
                        BrandName,
                        AlcoholContent
                    ));
                }
                else
                {
                    _adminService.SendNotification(
                        senderUserID: UserId,
                        title: "New Drink Request",
                        description: $"User requested to add drink: {DrinkName}"
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
