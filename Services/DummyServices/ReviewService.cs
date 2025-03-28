using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Services.DummyServies
{
    // Temp class until the other clandestine implements the model!

    public class Review
    {
        public int DrinkID { get; set; }
        public float Score { get; set; }
        public int ReviewerID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PostedDateTime { get; set; }
    }
    class ReviewService
    {
        private HashSet<Review> dummyReviews = new HashSet<Review>
        {
            new Review
            {
                DrinkID = 1,
                ReviewerID = 1,
                Title = "Awesome!",
                Description = "Loved it!",
                Score = 4.5f,
                PostedDateTime = DateTime.Now.AddDays(-2)
            },
            new Review
            {
                DrinkID = 1,
                ReviewerID = 2,
                Title = "Not bad",
                Description = "Decent flavor.",
                Score = 4.0f,
                PostedDateTime = DateTime.Now.AddDays(-1)
            },
            new Review
            {
                DrinkID = 2,
                ReviewerID = 3,
                Title = "Meh",
                Description = "Was okay.",
                Score = 2.5f,
                PostedDateTime = DateTime.Now
            }
        };

        public List<Review> GetReviewsByID(int drinkID)
        {
            return dummyReviews.Where(r => r.DrinkID == drinkID).ToList();
        }

        public float GetReviewAverageByID(int drinkID)
        {
            var reviews = GetReviewsByID(drinkID);
            if (reviews.Count == 0) return 0;
            return reviews.Average(r => r.Score);
        }
    }
}
