// <copyright file="Category.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Models
{
    using System;

    /// <summary>
    /// Represents a category assigned to a drink (e.g., beer, wine).
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="categoryId">Unique identifier for the category.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <exception cref="ArgumentException">Thrown when categoryName is null or empty.</exception>
        public Category(int categoryId, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException("Category name cannot be null or empty.", nameof(categoryName));
            }

            this.CategoryId = categoryId;
            this.CategoryName = categoryName;
        }

        /// <summary>
        /// Gets the unique identifier for the category.
        /// </summary>
        public int CategoryId { get; }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        public string CategoryName { get; }

        /// <summary>
        /// Checks if the current category is equal to another category.
        /// </summary>
        /// <param name="objectToCheck"> The object to be compared. </param>
        /// <returns> True, if equal, false otherwise. </returns>
        public override bool Equals(object? objectToCheck)
        {
            return objectToCheck is Category other && this.CategoryId == other.CategoryId;
        }

        /// <summary>
        /// Returns the hash code for the current category.
        /// </summary>
        /// <returns> Returns the int hash code for the current category. </returns>
        public override int GetHashCode() => this.CategoryId.GetHashCode();
    }
}