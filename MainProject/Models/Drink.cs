using System;
using System.Collections.Generic;

namespace WinUIApp.Models
{
    /// <summary>
    /// Represents a drink with associated brand, image, alcohol content, and categories.
    /// </summary>
    public class Drink
    {
        private const float MaxAlcoholContent = 100.0f;

        public int DrinkId { get; set; }

        private string? _drinkName;
        public string? DrinkName
        {
            get => _drinkName;
            set => _drinkName = value;
        }

        private string _drinkImageUrl = string.Empty;
        public string DrinkImageUrl
        {
            get => _drinkImageUrl;
            set => _drinkImageUrl = value ?? string.Empty;
        }

        private List<Category> _categoryList;
        public List<Category> CategoryList
        {
            get => _categoryList;
            set => _categoryList = value;
        }

        public Brand DrinkBrand { get; set; }

        private float _alcoholContent;
        public float AlcoholContent
        {
            get => _alcoholContent;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(AlcoholContent), "Alcohol content must be a positive value.");
                if (value > MaxAlcoholContent)
                    throw new ArgumentOutOfRangeException(nameof(AlcoholContent), $"Alcohol content must not exceed {MaxAlcoholContent}.");
                _alcoholContent = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Drink"/> class.
        /// </summary>
        /// <param name="id">Unique identifier for the drink.</param>
        /// <param name="drinkName">Name of the drink.</param>
        /// <param name="imageUrl">URL of the drink image.</param>
        /// <param name="categories">Categories associated with the drink.</param>
        /// <param name="brand">Brand of the drink.</param>
        /// <param name="alcoholContent">Alcohol content percentage.</param>
        /// <exception cref="ArgumentNullException">Thrown when brand is null.</exception>
        public Drink(int id, string? drinkName, string imageUrl, List<Category> categories, Brand brand, float alcoholContent)
        {
            DrinkId = id;
            DrinkName = drinkName;
            DrinkImageUrl = imageUrl;
            CategoryList = categories;
            DrinkBrand = brand ?? throw new ArgumentNullException(nameof(brand), "Brand cannot be null");
            AlcoholContent = alcoholContent;
        }
    }
}
