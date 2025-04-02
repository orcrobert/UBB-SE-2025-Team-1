using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;

namespace WinUIApp.ViewModels
{
    class MainPageViewModel 
    {
        private DrinkService drinkService = new DrinkService();
        private UserService userService = new UserService();

        public MainPageViewModel()
        {
        }

        public Drink getDrinkOfTheDay()
        {
            return drinkService.getDrinkOfTheDay();
        }
    }
    
    
}
