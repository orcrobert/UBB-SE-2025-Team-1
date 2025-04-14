// <copyright file="DrinkPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;

    /// <summary>
    /// ViewModel for the DrinkPage. Manages the state and behavior of the drink page, including adding/removing drinks from a user's personal list.
    /// </summary>
    public class DrinkPageViewModel : INotifyPropertyChanged
    {
        private readonly IDrinkService drinkService;
        private readonly IUserService userService;
        private int userId;
        private int drinkId;
        private bool isInUserDrinksList;
        private string buttonText;

        /// <summary>
        /// Event handler for property changes. This is used for data binding in the UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkPageViewModel"/> class.
        /// </summary>
        /// <param name="drinkService">Drink service. </param>
        /// <param name="userService"> User service. </param>
        public DrinkPageViewModel(IDrinkService drinkService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
            this.ButtonText = "\U0001F5A4";
            Debug.WriteLine("DrinkPageViewModel: Default constructor called");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkPageViewModel"/> class with a specific drink ID.
        /// </summary>
        /// <param name="drinkId">The ID of the drink to be managed.</param>
        /// /// <param name="drinkService">Drink service. </param>
        /// <param name="userService"> User service. </param>
        public DrinkPageViewModel(int drinkId, IDrinkService drinkService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
            this.drinkId = drinkId;
            this.ButtonText = "\U0001F5A4";
            Debug.WriteLine($"DrinkPageViewModel: Constructor called with DrinkId {drinkId}");
        }

        /// <summary>
        /// Gets or sets the user ID. This is used to identify the user who is interacting with the drink page.
        /// </summary>
        public int UserId
        {
            get => this.userId;
            set
            {
                if (this.userId != value)
                {
                    this.userId = value;
                    this.OnPropertyChanged();
                    Debug.WriteLine($"DrinkPageViewModel: UserId set to {this.userId}");
                    if (this.DrinkId > 0 && value > 0)
                    {
                        this.CheckIfInListAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the drink ID. This is used to identify the drink being managed by the view model.
        /// </summary>
        public int DrinkId
        {
            get => this.drinkId;
            set
            {
                if (this.drinkId != value)
                {
                    this.drinkId = value;
                    this.OnPropertyChanged();
                    Debug.WriteLine($"DrinkPageViewModel: DrinkId set to {this.drinkId}");
                    if (this.GetCurrentUserId() > 0 && value > 0)
                    {
                        this.CheckIfInListAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the drink is in the user's personal list.
        /// </summary>
        public string ButtonText
        {
            get => this.buttonText;
            set
            {
                if (this.buttonText != value)
                {
                    this.buttonText = value;
                    this.OnPropertyChanged();
                    Debug.WriteLine($"DrinkPageViewModel: ButtonText set to {this.buttonText}");
                }
            }
        }

        /// <summary>
        /// Checks if the drink is in the user's personal list.
        /// If the user ID or drink ID is invalid, sets _isInUserDrinksList to false.
        /// If valid, checks the drink list using the drink service.
        /// If the drink is found, sets _isInUserDrinksList to true and updates the button text accordingly.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CheckIfInListAsync()
        {
            Debug.WriteLine($"DrinkPageViewModel: CheckIfInListAsync called for User {this.GetCurrentUserId()}, Drink {this.DrinkId}");
            if (this.GetCurrentUserId() <= 0 || this.DrinkId <= 0)
            {
                this.isInUserDrinksList = false;
                this.UpdateButtonText();
                Debug.WriteLine($"DrinkPageViewModel: CheckIfInListAsync - User or Drink ID invalid, _isInUserDrinksList set to false");
                return;
            }

            try
            {
                this.isInUserDrinksList = await Task.Run(() => this.drinkService.IsDrinkInUserPersonalList(this.GetCurrentUserId(), this.DrinkId));
                this.UpdateButtonText();
                Debug.WriteLine($"DrinkPageViewModel: CheckIfInListAsync - _isInUserDrinksList is now {this.isInUserDrinksList}");
            }
            catch (Exception checkingDrinkListException)
            {
                Debug.WriteLine($"DrinkPageViewModel: Error checking drink list: {checkingDrinkListException.Message}");
            }
        }

        /// <summary>
        /// Adds or removes the drink from the user's personal list based on its current state.
        /// If the drink is already in the list, it will be removed; otherwise, it will be added.
        /// If the user ID or drink ID is invalid, the method will return without making any changes.
        /// </summary>
        /// <returns> task.</returns>
        public async Task AddRemoveFromListAsync()
        {
            Debug.WriteLine($"DrinkPageViewModel: AddRemoveFromListAsync called for User {this.GetCurrentUserId()}, Drink {this.DrinkId}. _isInUserDrinksList: {this.isInUserDrinksList}");
            if (this.GetCurrentUserId() <= 0 || this.DrinkId <= 0)
            {
                Debug.WriteLine($"DrinkPageViewModel: AddRemoveFromListAsync - User or Drink ID invalid, returning");
                return;
            }

            try
            {
                bool isOperationSuccessful;
                if (this.isInUserDrinksList)
                {
                    Debug.WriteLine($"DrinkPageViewModel: Removing Drink {this.DrinkId} for User {this.GetCurrentUserId()}");
                    isOperationSuccessful = await Task.Run(() => this.drinkService.DeleteFromUserPersonalDrinkList(this.GetCurrentUserId(), this.DrinkId));
                    if (isOperationSuccessful)
                    {
                        this.isInUserDrinksList = false;
                        Debug.WriteLine($"DrinkPageViewModel: Successfully removed Drink {this.DrinkId}");
                    }
                    else
                    {
                        Debug.WriteLine($"DrinkPageViewModel: Failed to remove Drink {this.DrinkId}");
                    }
                }
                else
                {
                    Debug.WriteLine($"DrinkPageViewModel: Adding Drink {this.DrinkId} for User {this.GetCurrentUserId()}");
                    isOperationSuccessful = await Task.Run(() => this.drinkService.AddToUserPersonalDrinkList(this.GetCurrentUserId(), this.DrinkId));
                    if (isOperationSuccessful)
                    {
                        this.isInUserDrinksList = true;
                        Debug.WriteLine($"DrinkPageViewModel: Successfully added Drink {this.DrinkId}");
                    }
                    else
                    {
                        Debug.WriteLine($"DrinkPageViewModel: Failed to add Drink {this.DrinkId}");
                    }
                }

                this.UpdateButtonText();
            }
            catch (Exception updateDrinkListException)
            {
                Debug.WriteLine($"DrinkPageViewModel: Error updating drink list: {updateDrinkListException.Message}");
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">String name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Gets the current user ID from the user service.
        /// </summary>
        /// <returns>Current user ID.</returns>
        private int GetCurrentUserId()
        {
            int userId = this.userService.GetCurrentUserId();
            Debug.WriteLine($"DrinkPageViewModel: GetCurrentUserId returned {userId}");
            return userId;
        }

        /// <summary>
        /// Internal method to update the button text based on the current state of _isInUserDrinksList.
        /// If the drink is in the user's list, the button text will be a heart symbol; otherwise, it will be a black heart symbol.
        /// </summary>
        private void UpdateButtonText()
        {
            this.ButtonText = this.isInUserDrinksList ? "\u2665" : "\U0001F5A4";
            Debug.WriteLine($"DrinkPageViewModel: ButtonText updated to {this.ButtonText} (_isInUserDrinksList: {this.isInUserDrinksList})");
        }
    }
}