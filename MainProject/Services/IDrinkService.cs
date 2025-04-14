// <copyright file="IDrinkService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Services
{
    using System.Collections.Generic;
    using WinUIApp.Models;

    /// <summary>
    /// Interface for managing drink-related operations.
    /// </summary>
    public interface IDrinkService
    {
        /// <summary>
        /// Adds a drink to the database.
        /// </summary>
        /// <param name="inputtedDrinkName"> Name. </param>
        /// <param name="inputtedDrinkPath"> ImagePath. </param>
        /// <param name="inputtedDrinkCategories"> Categories. </param>
        /// <param name="inputtedDrinkBrandName"> Brand. </param>
        /// <param name="inputtedAlcoholPercentage"> Alcohol. </param>
        /// <exception cref="Exception"> Any issues. </exception>
        void AddDrink(string inputtedDrinkName, string inputtedDrinkPath, List<Category> inputtedDrinkCategories, string inputtedDrinkBrandName, float inputtedAlcoholPercentage);

        /// <summary>
        /// Adds a drink to the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        bool AddToUserPersonalDrinkList(int userId, int drinkId);

        /// <summary>
        /// Deletes a drink from the database.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <exception cref="Exception"> Any issues. </exception>
        void DeleteDrink(int drinkId);

        /// <summary>
        /// Deletes a drink from the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        bool DeleteFromUserPersonalDrinkList(int userId, int drinkId);

        /// <summary>
        /// Retrieves a list of drink brands.
        /// </summary>
        /// <returns> List of brands. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        List<Brand> GetDrinkBrandNames();

        /// <summary>
        /// Gets the drink by ID.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> The drink. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        Drink? GetDrinkById(int drinkId);

        /// <summary>
        /// Retrieves a random drink ID from the database.
        /// </summary>
        /// <returns> List of categories. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        List<Category> GetDrinkCategories();

        /// <summary>
        /// Retrieves the drink of the day.
        /// </summary>
        /// <returns> The drink. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        Drink GetDrinkOfTheDay();

        /// <summary>
        /// Gets drinks based on various filters and ordering criteria.
        /// </summary>
        /// <param name="searchKeyword"> search term. </param>
        /// <param name="drinkBrandNameFilter"> brand filter. </param>
        /// <param name="drinkCategoryFilter"> category filter. </param>
        /// <param name="minimumAlcoholPercentage"> minimum alcohol content. </param>
        /// <param name="maximumAlcoholPercentage"> maximum alcohol content. </param>
        /// <param name="orderingCriteria"> order criteria. </param>
        /// <returns> List of drinks. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        List<Drink> GetDrinks(string? searchKeyword, List<string>? drinkBrandNameFilter, List<string>? drinkCategoryFilter, float? minimumAlcoholPercentage, float? maximumAlcoholPercentage, Dictionary<string, bool>? orderingCriteria);

        /// <summary>
        /// Retrieves a random drink ID from the database.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="maximumDrinkCount"> Not sure. </param>
        /// <returns> Personal list. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        List<Drink> GetUserPersonalDrinkList(int userId, int maximumDrinkCount = 1);

        /// <summary>
        /// Checks if a drink is already in the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> true, if yes, false otherwise. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        bool IsDrinkInUserPersonalList(int userId, int drinkId);

        /// <summary>
        /// Updates a drink in the database.
        /// </summary>
        /// <param name="drink"> Drink. </param>
        /// <exception cref="Exception"> Any issues. </exception>
        void UpdateDrink(Drink drink);

        /// <summary>
        /// Votes for the drink of the day.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> The drink. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        Drink VoteDrinkOfTheDay(int userId, int drinkId);
    }
}