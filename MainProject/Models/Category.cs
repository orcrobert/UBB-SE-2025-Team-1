using System;

namespace WinUIApp.Models
{
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="categoryId">Unique identifier for the category.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <exception cref="ArgumentException">Thrown when categoryName is null or empty.</exception>
        public Category(int categoryId, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name cannot be null or empty.", nameof(categoryName));

            CategoryId = categoryId;
            CategoryName = categoryName;
        }

        public override bool Equals(object? obj)
        {
            return obj is Category other && CategoryId == other.CategoryId;
        }

        public override int GetHashCode() => CategoryId.GetHashCode();
    }
}
