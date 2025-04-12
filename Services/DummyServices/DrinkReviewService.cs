using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

namespace WinUIApp.Services.DummyServies
{
    public class DrinkReviewService
    {
        private HashSet<Review> dummyReviews = new HashSet<Review>
{
    new Review(1, 4.5f, 1, "Fantastic!", "Absolutely loved it!", DateTime.Now.AddDays(-3)),
    new Review(2, 3.0f, 1, "Average", "Not the best, not the worst.", DateTime.Now.AddDays(-2)),
    new Review(3, 5.0f, 1, "Perfection", "Best drink ever!", DateTime.Now.AddDays(-1)),

    new Review(2, 4.0f, 2, "Pretty good", "Would buy again.", DateTime.Now.AddDays(-4)),
    new Review(3, 2.5f, 2, "Not for me", "Tastes weird.", DateTime.Now.AddDays(-3)),

    new Review(1, 3.5f, 3, "Decent", "Expected more.", DateTime.Now.AddDays(-5)),
    new Review(4, 4.8f, 3, "Amazing!", "Highly recommended.", DateTime.Now.AddDays(-2)),

    new Review(5, 1.5f, 4, "Terrible", "Would not recommend.", DateTime.Now.AddDays(-6)),
    new Review(2, 4.2f, 4, "Surprisingly good", "Really enjoyed this one.", DateTime.Now.AddDays(-3)),
    new Review(3, 3.7f, 4, "Not bad", "Pretty decent drink.", DateTime.Now.AddDays(-1)),

    new Review(4, 5.0f, 5, "My favorite!", "Can't get enough of this.", DateTime.Now.AddDays(-7)),
    new Review(6, 2.0f, 5, "Disappointed", "Didn't like the taste.", DateTime.Now.AddDays(-3)),

    new Review(3, 3.0f, 6, "Meh", "Just okay.", DateTime.Now.AddDays(-4)),
    new Review(5, 4.3f, 6, "Pretty good", "Would drink again.", DateTime.Now.AddDays(-2)),
    new Review(6, 4.0f, 6, "Liked it", "A solid choice.", DateTime.Now.AddDays(-1)),

    new Review(7, 4.6f, 7, "Awesome!", "One of the best.", DateTime.Now.AddDays(-5)),
    new Review(2, 2.8f, 7, "Eh", "Not a fan.", DateTime.Now.AddDays(-2)),

    new Review(1, 3.9f, 8, "Good", "I liked it.", DateTime.Now.AddDays(-6)),
    new Review(5, 4.5f, 8, "Excellent", "Great taste.", DateTime.Now.AddDays(-4)),
    new Review(7, 2.5f, 8, "Not great", "Wouldn't buy again.", DateTime.Now.AddDays(-1)),

    new Review(8, 5.0f, 9, "Top notch", "Best in class.", DateTime.Now.AddDays(-7)),
    new Review(3, 3.4f, 9, "Decent", "Not bad at all.", DateTime.Now.AddDays(-3)),
    new Review(2, 4.1f, 9, "Nice", "Would recommend.", DateTime.Now.AddDays(-1)),

    new Review(9, 2.0f, 10, "Nope", "Did not like it.", DateTime.Now.AddDays(-5)),
    new Review(4, 3.8f, 10, "Good enough", "Might try again.", DateTime.Now.AddDays(-2)),

    new Review(6, 4.7f, 11, "Superb", "Delicious.", DateTime.Now.AddDays(-6)),
    new Review(8, 2.3f, 11, "Not great", "Wouldn't recommend.", DateTime.Now.AddDays(-4)),
    new Review(7, 3.6f, 11, "Alright", "Nothing special.", DateTime.Now.AddDays(-2)),

    new Review(5, 5.0f, 12, "The best!", "Absolutely amazing.", DateTime.Now.AddDays(-7)),
    new Review(3, 3.0f, 12, "Okay", "Nothing remarkable.", DateTime.Now.AddDays(-5)),

    new Review(9, 1.5f, 13, "Horrible", "Worst drink ever.", DateTime.Now.AddDays(-6)),
    new Review(8, 4.2f, 13, "Pleasantly surprised", "Really nice drink.", DateTime.Now.AddDays(-3)),
    new Review(7, 4.0f, 13, "Pretty good", "Would have it again.", DateTime.Now.AddDays(-1))
};


        public List<Review> GetReviewsByID(int drinkID)
        {
            return dummyReviews.Where(r => r.DrinkId == drinkID).ToList();
        }

        public float GetReviewAverageByID(int drinkID)
        {
            var reviews = GetReviewsByID(drinkID);
            if (reviews.Count == 0) return 0;
            return reviews.Average(r => r.ReviewScore);
        }
    }
}
