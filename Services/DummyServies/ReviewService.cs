using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Services.DummyServies
{

    public class Review
    {
        public int Id { get; set; }
        public int DrinkId { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
    }
    class ReviewService
    {
        private List<Review> dummyReviews = new List<Review>
        {
            new Review { Id = 1, DrinkId = 1, Comment = "Great drink!", Rating = 4.5f },
            new Review { Id = 2, DrinkId = 1, Comment = "Pretty good.", Rating = 4.0f },
            new Review { Id = 3, DrinkId = 2, Comment = "Not my taste.", Rating = 2.5f }
        };

        public List<Review> GetReviewsByID(int drinkID)
        {
            return dummyReviews.Where(r => r.DrinkId == drinkID).ToList();
        }

        public float GetReviewAverageByID(int drinkID)
        {
            var reviews = GetReviewsByID(drinkID);
            if (reviews.Count == 0) return 0;
            return reviews.Average(r => r.Rating);
        }
    }
}
