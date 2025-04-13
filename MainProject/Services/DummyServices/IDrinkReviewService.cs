using System.Collections.Generic;
using WinUIApp.Models;

namespace WinUIApp.Services.DummyServices
{
    public interface IDrinkReviewService
    {
        float GetReviewAverageByID(int drinkID);
        List<Review> GetReviewsByID(int drinkID);
    }
}