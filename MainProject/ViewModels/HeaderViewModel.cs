// <copyright file="HeaderViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WinUIApp.Models;
    using WinUIApp.Services;

    /// <summary>
    /// ViewModel for the Header component. Manages the state and behavior of the header, including fetching drink categories.
    /// </summary>
    internal class HeaderViewModel
    {
        private IDrinkService drinkService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderViewModel"/> class.
        /// </summary>
        /// <param name="drinkService">The drink service used to manage drinks.</param>
        public HeaderViewModel(IDrinkService drinkService)
        {
            this.drinkService = drinkService;
        }

        /// <summary>
        /// Fetches the list of drink categories from the drink service.
        /// </summary>
        /// <returns>List of drink categories.</returns>
        public List<Category> GetCategories()
        {
            return this.drinkService.GetDrinkCategories();
        }
    }
}