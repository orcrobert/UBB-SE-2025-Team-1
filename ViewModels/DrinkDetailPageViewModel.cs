using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Management.Deployment;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.Views.Pages;

namespace WinUIApp.Views.ViewModels
{
    public class DrinkDetailPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DrinkService _drinkService;
        private readonly ReviewService _reviewService;
        private readonly UserService _userService;
        private readonly AdminService _adminService;


        private Drink _drink;
        public Drink Drink
        {
            get { return _drink; }
            set
            {
                _drink = value;
                OnPropertyChanged(nameof(Drink));
                OnPropertyChanged(nameof(CategoriesDisplay));
            }
        }

        public string CategoriesDisplay =>
        Drink?.Categories != null
        ? string.Join(", ", Drink.Categories.Select(c => c.Name))
        : string.Empty;

        private float _averageReviewScore;
        public float AverageReviewScore
        {
            get => _averageReviewScore;
            set
            {
                _averageReviewScore = value;
                OnPropertyChanged(nameof(AverageReviewScore));
            }
        }
        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        public DrinkDetailPageViewModel(DrinkService drinkService, ReviewService reviewService, UserService userService, AdminService adminService)
        {
            _drinkService = drinkService;
            _reviewService = reviewService;
            _userService = userService;
            _adminService = adminService;
        }

        public void LoadDrink(int drinkId)
        {
            Drink=_drinkService.getDrinkById(drinkId);
            AverageReviewScore=_reviewService.GetReviewAverageByID(drinkId);
            List<Review> reviews = _reviewService.GetReviewsByID(drinkId);
            Reviews.Clear();
            foreach (Review review in reviews)
            {
                Reviews.Add(review);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsCurrentUserAdmin()
        {
            return _adminService.IsAdmin(_userService.GetCurrentUserID());
        }

        public void RemoveDrink()
        {
            if (IsCurrentUserAdmin())
            {
                _drinkService.deleteDrink(Drink.Id);
            }
            else
            {
                _adminService.SendNotification(_userService.GetCurrentUserID(), "Removal of drink with id:"+Drink.Id+" and name:"+Drink.DrinkName, "User requested removal of drink from database.");
            }
        }

        public void VoteForDrink()
        {
            int userId = _userService.GetCurrentUserID();
            try
            {
                _drinkService.voteDrinkOfTheDay(Drink.Id, userId);
            }
            catch (Exception e)
            {
                throw new Exception("Error happened while voting for a drink:", e);
            }
        }
    }
}
