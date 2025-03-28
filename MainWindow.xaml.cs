using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Services;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WinUIApp.Services.DummyServies;

namespace WinUIApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            var dbService = DatabaseService.Instance;
            try
            {
                string brandName = brandTextBox.Text.Trim();
                string query = "INSERT INTO Brand (BrandName) VALUES (@BrandName)";

                var parameter = new List<MySqlParameter>
                {
                    new MySqlParameter("@BrandName", MySqlDbType.VarChar) { Value = brandName }
                };

                dbService.ExecuteQuery(query, parameter);
            }
            catch (Exception ex)
            {
                myTextBlock.Text = $"Error: {ex.Message}";
            }
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                myTextBlock.Text = "Connected to MySQL!";

                string query = "SELECT * FROM Brand ORDER BY BrandId;";
                var selectResult = dbService.ExecuteSelect(query);

                if (selectResult.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var row in selectResult)
                    {
                        int brandId = Convert.ToInt32(row["BrandId"]);
                        string brandName = row["BrandName"].ToString();

                        sb.AppendLine($"Brand: {brandName} (ID: {brandId})");
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

        private void displayCurrentUserButton_Click(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            currentUserTextBlock.Text = userService.GetCurrentUserID().ToString();
        }
    }
}
