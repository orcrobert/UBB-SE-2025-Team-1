// <copyright file="SearchPageNavigationParameters.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Utils.NavigationParameters
{
    using System.Collections.Generic;
    using WinUIApp.Models;

    /// <summary>
    /// Parameters for navigating to the search page.
    /// </summary>
    public class SearchPageNavigationParameters
    {
        /// <summary>
        /// Gets or sets the list of selected brands for filtering.
        /// </summary>
        public List<Category>? SelectedCategoryFilters { get; set; }

        /// <summary>
        /// Gets or sets the list of selected categories for filtering.
        /// </summary>
        public string? InputSearchKeyword { get; set; }
    }
}