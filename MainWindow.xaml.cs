using System;
using System.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;

namespace WinUIApp
{
    public sealed partial class MainWindow : Window
    {
        private readonly string _connectionString;

        public MainWindow()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                myTextBlock.Text = "Connection string is not loaded!";
                return;
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    myTextBlock.Text = "Connected to MySQL!";

                    string query = "SELECT * FROM Brand;";
                    using (var command = new MySqlCommand(query, connection))
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
            }
        }
    }
}
