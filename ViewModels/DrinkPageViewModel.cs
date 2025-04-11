using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using System.Diagnostics;

namespace WinUIApp.ViewModels
{
    /// <summary>
    /// ViewModel for the DrinkPage. Manages the state and behavior of the drink page, including adding/removing drinks from a user's personal list.
    /// </summary>
    public class DrinkPageViewModel : INotifyPropertyChanged
    {
        private readonly DrinkService _drinkService;
        private readonly UserService _userService;
        private int _userId;
        private int _drinkId;
        private bool _isInUserDrinksList;
        private string _buttonText;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Default constructor for the DrinkPageViewModel. Initializes the drink service and user service.
        /// </summary>
        public DrinkPageViewModel()
        {
            _drinkService = new DrinkService();
            _userService = new UserService();
            ButtonText = "\U0001F5A4";
            Debug.WriteLine("DrinkPageViewModel: Default constructor called");
        }

        /// <summary>
        /// Constructor for a specific drink ID. Initializes the drink service and user service, and sets the drink ID.
        /// </summary>
        /// <param name="drinkId">The ID of the drink to be managed.</param>
        public DrinkPageViewModel(int drinkId)
        {
            _drinkService = new DrinkService();
            _userService = new UserService();
            _drinkId = drinkId;
            ButtonText = "\U0001F5A4";
            Debug.WriteLine($"DrinkPageViewModel: Constructor called with DrinkId {drinkId}");
        }

        /// <summary>
        /// Event handler for updating the UI when user ID changes.
        /// </summary>
        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"DrinkPageViewModel: UserId set to {_userId}");
                    if (DrinkId > 0 && value > 0)
                    {
                        CheckIfInListAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for updating the UI when drink ID changes.
        /// </summary>
        public int DrinkId
        {
            get => _drinkId;
            set
            {
                if (_drinkId != value)
                {
                    _drinkId = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"DrinkPageViewModel: DrinkId set to {_drinkId}");
                    if (GetCurrentUserId() > 0 && value > 0)
                    {
                        CheckIfInListAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for updating the UI when the button text changes.
        /// </summary>
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (_buttonText != value)
                {
                    _buttonText = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"DrinkPageViewModel: ButtonText set to {_buttonText}");
                }
            }
        }

        /// <summary>
        /// Gets the current user ID from the user service.
        /// </summary>
        /// <returns>Current user ID.</returns>
        private int GetCurrentUserId()
        {
            int userId = _userService.GetCurrentUserID();
            Debug.WriteLine($"DrinkPageViewModel: GetCurrentUserId returned {userId}");
            return userId;
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
            Debug.WriteLine($"DrinkPageViewModel: CheckIfInListAsync called for User {GetCurrentUserId()}, Drink {DrinkId}");
            if (GetCurrentUserId() <= 0 || DrinkId <= 0)
            {
                _isInUserDrinksList = false;
                UpdateButtonText();
                Debug.WriteLine($"DrinkPageViewModel: CheckIfInListAsync - User or Drink ID invalid, _isInUserDrinksList set to false");
                return;
            }

            try
            {
                _isInUserDrinksList = await Task.Run(() => _drinkService.isDrinkInPersonalList(GetCurrentUserId(), DrinkId));
                UpdateButtonText();
                Debug.WriteLine($"DrinkPageViewModel: CheckIfInListAsync - _isInUserDrinksList is now {_isInUserDrinksList}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DrinkPageViewModel: Error checking drink list: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds or removes the drink from the user's personal list based on its current state.
        /// If the drink is already in the list, it will be removed; otherwise, it will be added.
        /// If the user ID or drink ID is invalid, the method will return without making any changes.
        /// </summary>
        /// <returns></returns>

        public async Task AddRemoveFromListAsync()
        {
            Debug.WriteLine($"DrinkPageViewModel: AddRemoveFromListAsync called for User {GetCurrentUserId()}, Drink {DrinkId}. _isInUserDrinksList: {_isInUserDrinksList}");
            if (GetCurrentUserId() <= 0 || DrinkId <= 0)
            {
                Debug.WriteLine($"DrinkPageViewModel: AddRemoveFromListAsync - User or Drink ID invalid, returning");
                return;
            }

            try
            {
                bool isOperationSuccessful;
                if (_isInUserDrinksList)
                {
                    Debug.WriteLine($"DrinkPageViewModel: Removing Drink {DrinkId} for User {GetCurrentUserId()}");
                    isOperationSuccessful = await Task.Run(() => _drinkService.deleteFromPersonalDrinkList(GetCurrentUserId(), DrinkId));
                    if (isOperationSuccessful)
                    {
                        _isInUserDrinksList = false;
                        Debug.WriteLine($"DrinkPageViewModel: Successfully removed Drink {DrinkId}");
                    }
                    else
                    {
                        Debug.WriteLine($"DrinkPageViewModel: Failed to remove Drink {DrinkId}");
                    }
                }
                else
                {
                    Debug.WriteLine($"DrinkPageViewModel: Adding Drink {DrinkId} for User {GetCurrentUserId()}");
                    isOperationSuccessful = await Task.Run(() => _drinkService.addToPersonalDrinkList(GetCurrentUserId(), DrinkId));
                    if (isOperationSuccessful)
                    {
                        _isInUserDrinksList = true;
                        Debug.WriteLine($"DrinkPageViewModel: Successfully added Drink {DrinkId}");
                    }
                    else
                    {
                        Debug.WriteLine($"DrinkPageViewModel: Failed to add Drink {DrinkId}");
                    }
                }
                UpdateButtonText();
            }
            catch (Exception UpdateDrinkListException)
            {
                Debug.WriteLine($"DrinkPageViewModel: Error updating drink list: {UpdateDrinkListException.Message}");
            }
        }

        /// <summary>
        /// Internal method to update the button text based on the current state of _isInUserDrinksList.
        /// If the drink is in the user's list, the button text will be a heart symbol; otherwise, it will be a black heart symbol.
        /// </summary>
        private void UpdateButtonText()
        {
            ButtonText = _isInUserDrinksList ? "\u2665" : "\U0001F5A4";
            Debug.WriteLine($"DrinkPageViewModel: ButtonText updated to {ButtonText} (_isInUserDrinksList: {_isInUserDrinksList})");
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">String name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}