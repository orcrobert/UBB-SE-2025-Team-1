using System;
using System.Threading.Tasks;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;

namespace WinUIApp.Views.ViewModels
{
    public class VoteButtonViewModel 
    {
        private readonly DrinkService _drinkService;
        private readonly UserService _userService;

        public VoteButtonViewModel(DrinkService drinkService, UserService userService)
        {
            _drinkService = drinkService;
            _userService = userService;
        }

        

    }
}