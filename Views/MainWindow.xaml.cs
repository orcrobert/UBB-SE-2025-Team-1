using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Services;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WinUIApp.Services.DummyServies;
using WinUIApp.Models;
using System.Collections.ObjectModel;

namespace WinUIApp.Views
{
    public sealed partial class MainWindow : Window
    {

        private ObservableCollection<Drink> DrinksList { get; set; } = new ObservableCollection<Drink>();

        public MainWindow()
        {
            this.InitializeComponent();
            SetFixedSize(1440, 900);
            MainFrame.Navigate(typeof(SearchPage));
        }

        private void SetFixedSize(int width, int height)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
        }

                    myTextBlock2.Text = sb.ToString();
                }
                else
                {
                    myTextBlock2.Text = "No data found!";
                }
            }
            catch (Exception ex)
            {
                myTextBlock.Text = $"Connection failed: {ex.Message}";
            }
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            AdminService adminService = new AdminService();
            ReviewService reviewService = new ReviewService();

            int userId = userService.GetCurrentUserID();
            string userType = adminService.IsAdmin(userId) ? "admin" : "not admin";

            List<Review> reviews = reviewService.GetReviewsByID(1);

            Review review = reviews[0];

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Title: {review.Title}");
            sb.AppendLine($"Reviewer ID: {review.ReviewerID}");
            sb.AppendLine($"Drink ID: {review.DrinkID}");
            sb.AppendLine($"Score: {review.Score}/5");
            sb.AppendLine($"Review: {review.Description}");
            sb.AppendLine($"Posted: {review.PostedDateTime.ToString("yyyy-MM-dd HH:mm")}");
            sb.AppendLine(new string('-', 30));

            currentUserTextBlock.Text = $"User with id {userId.ToString()} ({userType})";
            reviewTextBlock.Text = sb.ToString();

            adminService.SendNotification(1, "test", "test");

            Category category1 = new Category(1, "Brown Beer");
            Category category2 = new Category(2, "Strong Beer");
            List<Category> categories = new List<Category>();
            categories.Add(category1);
            categories.Add(category2);
            Brand brand = new Brand(100, "Chimay");
            Drink drink = new Drink(100, categories, brand, (float)9.0);

            drink.AlcoholContent = (float)12;

            sb = new StringBuilder();

            sb.AppendLine($"Drink ID: {drink.Id}");
            sb.AppendLine($"Brand: {drink.Brand.Name}");
            sb.AppendLine("Categories:");
            foreach (var category in drink.Categories)
                sb.AppendLine($"  - {category.Name}");
            sb.AppendLine($"Alcohol Content: {drink.AlcoholContent}%");
            sb.AppendLine(new string('-', 30));

            testModelsTextBlock.Text = sb.ToString();
        }

    }
}
