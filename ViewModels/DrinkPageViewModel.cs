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
    public class DrinkPageViewModel : INotifyPropertyChanged
    {
        private readonly DrinkService _drinkService;
        private readonly UserService _userService;
        private int _userId;
        private int _drinkId;
        private bool _isInList;
        private string _buttonText;

        public event PropertyChangedEventHandler PropertyChanged;

        public DrinkPageViewModel()
        {
            _drinkService = new DrinkService();
            _userService = new UserService();
            ButtonText = "\U0001F5A4";
            Debug.WriteLine("DrinkPageViewModel: Default constructor called");
        }

        public DrinkPageViewModel(int drinkId)
        {
            _drinkService = new DrinkService();
            _userService = new UserService();
            _drinkId = drinkId;
            ButtonText = "\U0001F5A4";
            Debug.WriteLine($"DrinkPageViewModel: Constructor called with DrinkId {drinkId}");
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
                    Debug.WriteLine($"DrinkPageViewModel: UserId set to {_userId}");
                    if (DrinkId > 0 && value > 0)
                    {
                        CheckIfInListAsync();
                    }
                }
            }
        }

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

        private int GetCurrentUserId()
        {
            int userId = _userService.GetCurrentUserId();
            Debug.WriteLine($"DrinkPageViewModel: GetCurrentUserId returned {userId}");
            return userId;
        }

        public async Task CheckIfInListAsync()
        {
            Debug.WriteLine($"DrinkPageViewModel: CheckIfInListAsync called for User {GetCurrentUserId()}, Drink {DrinkId}");
            if (GetCurrentUserId() <= 0 || DrinkId <= 0)
            {
                _isInList = false;
                UpdateButtonText();
                Debug.WriteLine($"DrinkPageViewModel: CheckIfInListAsync - User or Drink ID invalid, _isInList set to false");
                return;
            }

            try
            {
                _isInList = await Task.Run(() => _drinkService.IsDrinkInUserPersonalList(GetCurrentUserId(), DrinkId));
                UpdateButtonText();
                Debug.WriteLine($"DrinkPageViewModel: CheckIfInListAsync - _isInList is now {_isInList}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DrinkPageViewModel: Error checking drink list: {ex.Message}");
            }
        }

        public async Task AddRemoveFromListAsync()
        {
            Debug.WriteLine($"DrinkPageViewModel: AddRemoveFromListAsync called for User {GetCurrentUserId()}, Drink {DrinkId}. _isInList: {_isInList}");
            if (GetCurrentUserId() <= 0 || DrinkId <= 0)
            {
                Debug.WriteLine($"DrinkPageViewModel: AddRemoveFromListAsync - User or Drink ID invalid, returning");
                return;
            }

            try
            {
                bool success;
                if (_isInList)
                {
                    Debug.WriteLine($"DrinkPageViewModel: Removing Drink {DrinkId} for User {GetCurrentUserId()}");
                    success = await Task.Run(() => _drinkService.DeleteFromUserPersonalDrinkList(GetCurrentUserId(), DrinkId));
                    if (success)
                    {
                        _isInList = false;
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
                    success = await Task.Run(() => _drinkService.AddToUserPersonalDrinkList(GetCurrentUserId(), DrinkId));
                    if (success)
                    {
                        _isInList = true;
                        Debug.WriteLine($"DrinkPageViewModel: Successfully added Drink {DrinkId}");
                    }
                    else
                    {
                        Debug.WriteLine($"DrinkPageViewModel: Failed to add Drink {DrinkId}");
                    }
                }
                UpdateButtonText();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DrinkPageViewModel: Error updating drink list: {ex.Message}");
            }
        }

        private void UpdateButtonText()
        {
            ButtonText = _isInList ? "\u2665" : "\U0001F5A4";
            Debug.WriteLine($"DrinkPageViewModel: ButtonText updated to {ButtonText} (_isInList: {_isInList})");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}