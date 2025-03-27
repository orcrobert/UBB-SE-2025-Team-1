using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Services;
using MySql.Data.MySqlClient;

namespace WinUIApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                dbService.OpenConnection();
                myTextBlock.Text = "Connected to MySQL!";

                string query = "SELECT * FROM Brand;";
                using (var command = new MySqlCommand(query, dbService.GetConnection()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int brandId = reader.GetInt32(0);
                            string brandName = reader.GetString(1);
                            myTextBlock2.Text = $"Brand: {brandName} (ID: {brandId})";
                        }
                        else
                        {
                            myTextBlock2.Text = "No data found!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                myTextBlock.Text = $"Connection failed: {ex.Message}";
            }
            finally
            {
                dbService.CloseConnection();
            }
        }
    }
}
