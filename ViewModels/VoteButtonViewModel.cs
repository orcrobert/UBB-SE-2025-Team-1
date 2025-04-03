using System;
using System.Threading.Tasks;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;

namespace WinUIApp.Views.ViewModels
{
    public class VoteButtonViewModel 
    {
        private readonly DrinkService _drinkService = new DrinkService();
        private readonly UserService _userService = new UserService();

        public void VoteForDrink(int drinkId)
        {
            int userId = _userService.GetCurrentUserID();
            drinkId = 1;
            try
            {
                _drinkService.voteDrinkOfTheDay(drinkId, userId);
            }
            catch (Exception e)
            {
                throw new Exception("Error happened while voting for a drink:", e);
            }
        }

    }
}