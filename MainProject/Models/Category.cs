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
        /// Gets the unique identifier for the category.
        /// </summary>
        public int CategoryId { get; }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        public string CategoryName { get; }

        public override bool Equals(object? obj)
        {
            return obj is Category other && CategoryId == other.CategoryId;
        }

        public override int GetHashCode() => CategoryId.GetHashCode();
    }
}
