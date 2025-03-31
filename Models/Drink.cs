using System;
using System.Collections.Generic;

namespace WinUIApp.Models
{
    public class Drink
    {
        private int _id;
        private string? _drinkName;
        private string _drinkURL;
        private List<Category> _categories;
        private Brand _brand;
        private float _alcoholContent;

        public Drink(int id, string? drinkName, string drinkURL, List<Category> categories, Brand brand, float alcoholContent)
        {
            _id = id;
            _drinkName = drinkName;
            _drinkURL = drinkURL ?? string.Empty;
            _categories = categories ?? new List<Category>();
            _brand = brand;
            AlcoholContent = alcoholContent;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string? DrinkName
        {
            get { return _drinkName; }
            set { _drinkName = value; }
        }

        public string DrinkURL
        {
            get { return _drinkURL; }
            set { _drinkURL = value ?? string.Empty; }
        }

        public List<Category> Categories
        {
            get { return _categories; }
            set { _categories = value ?? new List<Category>(); }
        }

        public Brand Brand
        {
            get { return _brand; }
            set { _brand = value; }
        }

        public float AlcoholContent
        {
            get { return _alcoholContent; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(AlcoholContent), "Alcohol content must be a positive value.");
                else if (value > 100)
                    throw new ArgumentOutOfRangeException(nameof(AlcoholContent), "Alcohol content must be less than 100.");

                _alcoholContent = value;
            }
        }
    }
}