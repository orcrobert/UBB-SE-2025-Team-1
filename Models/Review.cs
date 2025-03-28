using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Models
{
    class Review
    {
        private int _drinkID;
        private float _score;
        private int _reviewerID;
        private string _title;
        private string _description;
        private DateTime _postedDateTime;

        public Review(int drinkID, float score, int reviewerID, string title, string description, DateTime postedDateTime)
        {
            _drinkID = drinkID;
            _score = score;
            _reviewerID = reviewerID;
            _title = title;
            _description = description;
            _postedDateTime = postedDateTime;
        }

        public int DrinkID
        {
            get { return _drinkID; }
            set { _drinkID = value; }
        }

        public float Score
        {
            get { return _score; }
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(Score), "Score must be between 0 and 5");
                _score = value;
            }
        }

        public int ReviewerID
        {
            get { return _reviewerID; }
            set { _reviewerID = value; }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Title cannot be null or empty", nameof(Title));
                _title = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Description cannot be null or empty", nameof(Description));
                _description = value;
            }
        }

        public DateTime PostedDateTime
        {
            get { return _postedDateTime; }
            set { _postedDateTime = value; }
        }
    }
}
