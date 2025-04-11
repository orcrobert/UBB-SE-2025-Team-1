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
    /// <summary>
    /// ViewModel for the DrinkDetailPage. Shows detailed information about a drink, including name, image, alcohol content, categories, and reviews.
    /// </summary>
    public class DrinkDetailPageViewModel : INotifyPropertyChanged
    {
        private const string CategorySeparator = ", ";

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DrinkService _drinkService;
        private readonly ReviewService _reviewService;
        private readonly UserService _userService;
        private readonly AdminService _adminService;


        private Drink _drink;

        /// <summary>
        /// The drink object that is being displayed in the detail page.
        /// </summary>
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

        /// <summary>
        /// A string representation of the drink's categories, used for display purposes.
        /// </summary>
        /// <returns>A comma-separated string of category names.</returns>
        public string CategoriesDisplay
        {
            get
            {
                if (Drink?.CategoryList != null)
                {
                    return string.Join(CategorySeparator, Drink.CategoryList.Select(drinkCategory => drinkCategory.CategoryName));
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private float _averageReviewScore;

        /// <summary>
        /// Gets or sets the average review score of the drink.
        /// </summary>
        public float AverageReviewScore
        {
            get => _averageReviewScore;
            set
            {
                _averageReviewScore = value;
                OnPropertyChanged(nameof(AverageReviewScore));
            }
        }

        /// <summary>
        /// A collection of reviews for the drink. Used to display user feedback and ratings.
        /// </summary>
        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkDetailPageViewModel"/> class.
        /// </summary>
        /// <param name="drinkService">The drink service used to manage drinks.</param>
        /// <param name="reviewService">The review service used to manage reviews.</param>
        /// <param name="userService">The user service used to manage users.</param>
        /// <param name="adminService">The admin service used to manage admin actions.</param>
        public DrinkDetailPageViewModel(DrinkService drinkService, ReviewService reviewService, UserService userService, AdminService adminService)
        {
            _drinkService = drinkService;
            _reviewService = reviewService;
            _userService = userService;
            _adminService = adminService;
        }

        /// <summary>
        /// Loads the drink details and reviews based on the provided drink ID.
        /// </summary>
        /// <param name="drinkId">The ID of the drink to load.</param>
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
        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Checks if the current user is an admin.
        /// </summary>
        /// <returns>True if the user is an admin; otherwise, false.</returns>
        public bool IsCurrentUserAdmin()
        {
            return _adminService.IsAdmin(_userService.GetCurrentUserID());
        }

        /// <summary>
        /// Removes the drink from the database. If the user is not an admin, a notification is sent to the admin for review.
        /// </summary>
        public void RemoveDrink()
        {
            if (IsCurrentUserAdmin())
            {
                _drinkService.deleteDrink(Drink.DrinkId);
            }
            else
            {
                _adminService.SendNotification(_userService.GetCurrentUserID(), "Removal of drink with id:"+Drink.DrinkId +" and name:"+Drink.DrinkName, "User requested removal of drink from database.");
            }
        }
        /// <summary>
        /// Votes for the drink of the day. This method checks if the user is an admin and if the drink is already voted for. If not, it allows the user to vote.
        /// </summary>
        /// <exception cref="Exception">Thrown if an error occurs while voting for the drink.</exception>
        public void VoteForDrink()
        {
            int userId = _userService.GetCurrentUserID();
            try
            {
                _drinkService.voteDrinkOfTheDay(Drink.DrinkId, userId);
            }
            catch (Exception VoteForDrinkException)
            {
                throw new Exception("Error happened while voting for a drink:", VoteForDrinkException);
            }
        }
    }
}
