using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

namespace WinUIApp.Services.DummyServies
{
    class ReviewService
    {
        private HashSet<Review> dummyReviews = new HashSet<Review>
        {
            new Review
            (
                drinkID: 1,
                score: 4.5f,
                reviewerID: 1,
                title: "Awesome!",
                description: "Loved it!",
                postedDateTime: DateTime.Now.AddDays(-2)
            ),
            new Review
            (
                drinkID: 1,
                score: 4.0f,
                reviewerID: 2,
                title: "Not bad",
                description: "Decent flavor.",
                postedDateTime: DateTime.Now.AddDays(-1)
            ),
            new Review
            (
                drinkID: 2,
                score: 2.5f,
                reviewerID: 3,
                title: "Meh",
                description: "Was okay.",
                postedDateTime: DateTime.Now
            )
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
