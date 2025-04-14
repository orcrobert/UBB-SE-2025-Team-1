// <copyright file="Review.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Models
{
    using System;

    /// <summary>
    /// Represents a user review for a drink.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Review"/> class.
        /// </summary>
        /// <param name="drinkId">The ID of the reviewed drink.</param>
        /// <param name="reviewScore">The score given (0–5).</param>
        /// <param name="reviewerUserId">The ID of the reviewer.</param>
        /// <param name="reviewTitle">Title of the review.</param>
        /// <param name="reviewDescription">Description/body of the review.</param>
        /// <param name="reviewPostedDateTime">Date the review was posted.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if score is not between 0 and 5.</exception>
        /// <exception cref="ArgumentException">Thrown if title or description is null/empty.</exception>
        public Review(int drinkId, float reviewScore, int reviewerUserId, string reviewTitle, string reviewDescription, DateTime reviewPostedDateTime)
        {
            if (reviewScore < 0 || reviewScore > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(reviewScore), "Score must be between 0 and 5.");
            }

            if (string.IsNullOrWhiteSpace(reviewTitle))
            {
                throw new ArgumentException("Review title cannot be null or empty.", nameof(reviewTitle));
            }

            if (string.IsNullOrWhiteSpace(reviewDescription))
            {
                throw new ArgumentException("Review description cannot be null or empty.", nameof(reviewDescription));
            }

            this.DrinkId = drinkId;
            this.ReviewScore = reviewScore;
            this.ReviewerUserId = reviewerUserId;
            this.ReviewTitle = reviewTitle;
            this.ReviewDescription = reviewDescription;
            this.ReviewPostedDateTime = reviewPostedDateTime;
        }

        /// <summary>
        /// Gets the ID of the reviewed drink.
        /// </summary>
        public int DrinkId { get; }

        /// <summary>
        /// Gets the score given to the drink (0 to 5).
        /// </summary>
        public float ReviewScore { get; }

        /// <summary>
        /// Gets the user ID of the reviewer.
        /// </summary>
        public int ReviewerUserId { get; }

        /// <summary>
        /// Gets the title of the review.
        /// </summary>
        public string ReviewTitle { get; }

        /// <summary>
        /// Gets the description/body of the review.
        /// </summary>
        public string ReviewDescription { get; }

        /// <summary>
        /// Gets the date and time the review was posted.
        /// </summary>
        public DateTime ReviewPostedDateTime { get; }
    }
}