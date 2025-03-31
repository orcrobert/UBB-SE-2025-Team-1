using WinUIApp.Models;

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public class DrinkDisplayItem
    {
        public Drink Drink { get; set; }
        public float AverageReviewScore { get; set; }

        public DrinkDisplayItem(Drink drink, float averageReviewScore)
        {
            Drink = drink;
            AverageReviewScore = averageReviewScore;
        }
    }
}
