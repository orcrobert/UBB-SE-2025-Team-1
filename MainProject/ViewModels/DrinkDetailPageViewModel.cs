// <copyright file="DrinkDetailPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.ViewModels
{
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
    using WinUIApp.Services.DummyServices;
    using WinUIApp.Views.Pages;

    /// <summary>
    /// ViewModel for the DrinkDetailPage. Shows detailed information about a drink, including name, image, alcohol content, categories, and reviews.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DrinkDetailPageViewModel"/> class.
    /// </remarks>
    /// <param name="drinkService">The drink service used to manage drinks.</param>
    /// <param name="reviewService">The review service used to manage reviews.</param>
    /// <param name="userService">The user service used to manage users.</param>
    /// <param name="adminService">The admin service used to manage admin actions.</param>
    public partial class DrinkDetailPageViewModel(DrinkService drinkService, DrinkReviewService reviewService, UserService userService, AdminService adminService) : INotifyPropertyChanged
    {
        private const string CategorySeparator = ", ";
        private readonly DrinkService drinkService = drinkService;
        private readonly DrinkReviewService reviewService = reviewService;
        private readonly UserService userService = userService;
        private readonly AdminService adminService = adminService;
        private Drink drink;
        private float averageReviewScore;

        /// <summary>
        /// Event handler for property changes. This is used for data binding in the UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the drink object. This contains all the details about the drink, including name, image URL, alcohol content, and categories.
        /// </summary>
        public Drink Drink
        {
            get
            {
                return this.drink;
            }

            set
            {
                this.drink = value;
                this.OnPropertyChanged(nameof(this.Drink));
                this.OnPropertyChanged(nameof(this.CategoriesDisplay));
            }
        }

        /// <summary>
        /// Gets or sets the name of the drink. This is used for display purposes.
        /// </summary>
        /// <returns>A comma-separated string of category names.</returns>
        public string CategoriesDisplay
        {
            get
            {
                if (this.Drink?.CategoryList != null)
                {
                    return string.Join(CategorySeparator, this.Drink.CategoryList.Select(drinkCategory => drinkCategory.CategoryName));
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the average review score of the drink.
        /// </summary>
        public float AverageReviewScore
        {
            get => this.averageReviewScore;
            set
            {
                this.averageReviewScore = value;
                this.OnPropertyChanged(nameof(this.AverageReviewScore));
            }
        }

        /// <summary>
        /// Gets or sets the list of reviews for the drink. This is an observable collection that allows for dynamic updates to the UI when reviews are added or removed.
        /// </summary>
        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        /// <summary>
        /// Loads the drink details and reviews based on the provided drink ID.
        /// </summary>
        /// <param name="drinkId">The ID of the drink to load.</param>
        public void LoadDrink(int drinkId)
        {
            this.Drink = this.drinkService.GetDrinkById(drinkId);
            this.AverageReviewScore = this.reviewService.GetReviewAverageByID(drinkId);
            List<Review> reviews = this.reviewService.GetReviewsByID(drinkId);
            this.Reviews.Clear();
            foreach (Review review in reviews)
            {
                this.Reviews.Add(review);
            }
        }

        /// <summary>
        /// Checks if the current user is an admin.
        /// </summary>
        /// <returns>True if the user is an admin; otherwise, false.</returns>
        public bool IsCurrentUserAdmin()
        {
            return this.adminService.IsAdmin(this.userService.GetCurrentUserId());
        }

        /// <summary>
        /// Removes the drink from the database. If the user is not an admin, a notification is sent to the admin for review.
        /// </summary>
        public void RemoveDrink()
        {
            if (this.IsCurrentUserAdmin())
            {
                this.drinkService.DeleteDrink(this.Drink.DrinkId);
            }
            else
            {
                this.adminService.SendNotificationFromUserToAdmin(this.userService.GetCurrentUserId(), "Removal of drink with id:" + this.Drink.DrinkId + " and name:" + this.Drink.DrinkName, "User requested removal of drink from database.");
            }
        }

        /// <summary>
        /// Votes for the drink of the day. This method checks if the user is an admin and if the drink is already voted for. If not, it allows the user to vote.
        /// </summary>
        /// <exception cref="Exception">Thrown if an error occurs while voting for the drink.</exception>
        public void VoteForDrink()
        {
            int userId = this.userService.GetCurrentUserId();
            try
            {
                this.drinkService.VoteDrinkOfTheDay(userId, this.Drink.DrinkId);
            }
            catch (Exception voteForDrinkException)
            {
                throw new Exception("Error happened while voting for a drink:", voteForDrinkException);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}