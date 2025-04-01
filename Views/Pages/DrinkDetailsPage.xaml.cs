using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;

namespace WinUIApp.Views.Pages
{
    public sealed partial class DrinkDetailsPage : Page
    {
        // Keep a reference to the current Drink so that event handlers can access it.
        private Drink _currentDrink;

        public DrinkDetailsPage()
        {
            this.InitializeComponent();
            LoadDrinkDetails(100); // Example drink ID
        }

        private void LoadDrinkDetails(int drinkId)
        {
            // Create sample data for demonstration purposes.

            // Create a sample Drink (all required parameters must be provided)
            _currentDrink = new Drink(
                drinkId,
                "Chimay Blue",                    // DrinkName
                "https://encrypted-tbn2.gstatic.com/shopping?q=tbn:ANd9GcRYdDUoOSYI4hRX-iPz-RyF8YEj9vx_tEDc3wTnOTnOMN8iIjfE-1UTfALbnNK2U3LHPl9MtSKmpoMmmNY-Dl693o66lZtPBcPruZ6hM4PhrftMeg1sXyerCv1ltZ9pXAD1Nw&usqp=CAc", // DrinkURL (remote image) or use "ms-appx:///Assets/YourImage.png" for local
                new List<Category>
                {
                    new Category(1, "Brown Beer"),
                    new Category(2, "Strong Beer")
                },
                new Brand(100, "Chimay"),
                12.0f
            );

            // Create a list of sample reviews for this drink.
            var sampleReviews = new List<Review>
            {
                new Review(drinkId, 4.0f, 101, "Great Beer", "Tasty and robust flavor!", DateTime.Now.AddDays(-2)),
                new Review(drinkId, 5.0f, 102, "Excellent", "Absolutely loved it!", DateTime.Now.AddDays(-1)),
                new Review(drinkId, 3.5f, 103, "Good", "Not the best but enjoyable.", DateTime.Now.AddDays(-3))
            };

            // Calculate the average review score.
            float avgScore = sampleReviews.Any() ? sampleReviews.Average(r => r.Score) : 0f;

            // Update UI controls with the drink details.
            DrinkNameTextBlock.Text = $"Name: {_currentDrink.DrinkName}";
            BrandTextBlock.Text = $"Brand: {_currentDrink.Brand.Name}";
            AlcoholContentTextBlock.Text = $"Alcohol Content: {_currentDrink.AlcoholContent}%";
            CategoryTextBlock.Text = $"Categories: {string.Join(", ", _currentDrink.Categories.Select(c => c.Name))}";
            AverageScoreTextBlock.Text = $"Average Score: {avgScore:0.0}/5";

            // Set the drink image.
            DrinkImage.Source = new BitmapImage(new Uri(_currentDrink.DrinkURL));

            // Bind the reviews list to the ListView.
            ReviewsListView.ItemsSource = sampleReviews;
        }

        // ---------------------
        // Event Handlers
        // ---------------------

        private void AddDrinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // TODO: Add logic to save _currentDrink to a personal drink list.
        }

        private void RemoveDrinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // TODO: Add logic to remove _currentDrink from the personal drink list.
        }

        private void VoteDrinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // TODO: Add logic to vote for this drink as the "drink of the day."
        }

        private void UpdateDrinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // TODO: Add logic to update the drink details.
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Navigate back if possible.
            if (this.Frame != null && this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
            else
            {

                // Create a new MainWindow
                var newWindow = new WinUIApp.Views.MainWindow();
                newWindow.Activate();

                // Close the current window (optional but prevents having multiple open)
                Microsoft.UI.Xaml.Window.Current.Close();
            }
        }
    }
}
