using System;

namespace WinUIApp.Models
{
    /// <summary>
    /// Represents a drink brand.
    /// </summary>
    public class Brand
    {
        /// <summary>
        /// Gets or sets the unique identifier for the brand.
        /// </summary>
        public int BrandId { get; set; }

        private string _brandName;

        /// <summary>
        /// Gets or sets the name of the brand.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when value is null or empty.</exception>
        public string BrandName
        {
            get => _brandName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Brand name cannot be null or empty.", nameof(BrandName));
                _brandName = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Brand"/> class.
        /// </summary>
        /// <param name="brandId">Unique identifier for the brand.</param>
        /// <param name="brandName">Name of the brand.</param>
        /// <exception cref="ArgumentException">Thrown when brandName is null or empty.</exception>
        public Brand(int brandId, string brandName)
        {
            BrandId = brandId;
            BrandName = brandName; // validation happens in setter
        }
    }
}
