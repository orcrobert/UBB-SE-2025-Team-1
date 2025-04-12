using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Models;
using WinUIApp.Services;

namespace WinUIApp.Views.ViewModels
{
    /// <summary>
    /// ViewModel for the Header component. Manages the state and behavior of the header, including fetching drink categories.
    /// </summary>
    class HeaderViewModel
    {
        private DrinkService _drinkService;

        /// <summary>
        /// Constructor for the HeaderViewModel. Initializes the drink service.
        /// </summary>
        /// <param name="drinkService">The drink service used to manage drinks.</param>
        public HeaderViewModel(DrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        /// <summary>
        /// Fetches the list of drink categories from the drink service.
        /// </summary>
        /// <returns>List of drink categories.</returns>
        public List<Category> GetCategories()
        {
            return _drinkService.GetDrinkCategories();
        }

    }
}
