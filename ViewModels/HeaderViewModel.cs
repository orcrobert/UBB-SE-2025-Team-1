using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Models;
using WinUIApp.Services;

namespace WinUIApp.Views.ViewModels
{
    class HeaderViewModel
    {
        private DrinkService _drinkService;

        public HeaderViewModel(DrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        public List<Category> GetCategories()
        {
            return _drinkService.GetDrinkCategories();
        }

    }
}
