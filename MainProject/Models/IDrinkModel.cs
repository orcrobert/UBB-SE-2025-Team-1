// <copyright file="IDrinkModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for managing drink-related operations.
    /// </summary>
    public interface IDrinkModel
    {
        /// <summary>
        /// Adds a new drink to the database.
        /// </summary>
        /// <param name="drinkName"> Drink name. </param>
        /// <param name="drinkUrl"> Drink Url. </param>
        /// <param name="categories"> List of categories. </param>
        /// <param name="brandName"> Brand name. </param>
        /// <param name="alcoholContent"> Alcohol content. </param>
        void AddDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent);

        /// <summary>
        /// Adds a drink to the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        bool AddToPersonalDrinkList(int userId, int drinkId);

        /// <summary>
        /// Deletes a drink from the database.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        void DeleteDrink(int drinkId);

        /// <summary>
        /// Deletes a drink from the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        bool DeleteFromPersonalDrinkList(int userId, int drinkId);

        /// <summary>
        /// Retrieves the drink of the day.
        /// </summary>
        /// <returns> Drink of the day id. </returns>
        int GetCurrentTopVotedDrink();

        /// <summary>
        /// Retrieves a list of all available drink brands.
        /// </summary>
        /// <returns> List of all brands. </returns>
        List<Brand> GetDrinkBrands();

        /// <summary>
        /// Retrieves a drink by its unique identifier.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> The drink. </returns>
        Drink? GetDrinkById(int drinkId);

        /// <summary>
        /// Retrieves a list of all available drink categories.
        /// </summary>
        /// <returns> List of all categories. </returns>
        List<Category> GetDrinkCategories();

        /// <summary>
        /// Retrieves a list of all available drinks.
        /// </summary>
        /// <returns> Drink of the day. </returns>
        Drink GetDrinkOfTheDay();

        /// <summary>
        /// Retrieves a list of drinks based on various filters and sorting options.
        /// </summary>
        /// <param name="searchTerm"> search term. </param>
        /// <param name="brandNameFilters"> brand filters. </param>
        /// <param name="categoryFilters"> category filters. </param>
        /// <param name="minimumAlcohool"> minimum alcohol content. </param>
        /// <param name="maximumAlcohool"> maximum alcohol content. </param>
        /// <param name="orderBy"> ordering parameter. </param>
        /// <returns> List of drinks. </returns>
        List<Drink> GetDrinks(string? searchTerm, List<string>? brandNameFilters, List<string>? categoryFilters, float? minimumAlcohool, float? maximumAlcohool, Dictionary<string, bool>? orderBy);

        /// <summary>
        /// Retrieves a list of drinks based on the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="numberOfDrinks"> Idk. </param>
        /// <returns> The list of drinks for the user. </returns>
        List<Drink> GetPersonalDrinkList(int userId, int numberOfDrinks = 1);

        /// <summary>
        /// Retrieves a random drink id from the database.
        /// </summary>
        /// <returns> Random drink id. </returns>
        int GetRandomDrinkId();

        /// <summary>
        /// Checks if a drink is already in the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if it is in the list, false otherwise. </returns>
        bool IsDrinkInPersonalList(int userId, int drinkId);

        /// <summary>
        /// Updates the details of an existing drink in the database.
        /// </summary>
        /// <param name="drink"> The drink with updated info. </param>
        void UpdateDrink(Drink drink);

        /// <summary>
        /// Votes for a drink of the day.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        void VoteDrinkOfTheDay(int userId, int drinkId);
    }
}