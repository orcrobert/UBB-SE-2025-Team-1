using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Models
{
    public class Drink
    {
        private int _id;
        private List<Category> _categories;
        private Brand _brand;
        private float _alcoholContent;

        public Drink(int id, List<Category> categories, Brand brand, float alcoholContent)
        {
            _id = id;
            _categories = categories ?? new List<Category>();
            _brand = brand;
            _alcoholContent = alcoholContent;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
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

        public string FormattedAlcoholContent
        {
            get { return $"{AlcoholContent:F1}%"; }
        }
    }
}
