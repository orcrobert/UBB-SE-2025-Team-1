using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Services;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

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
                var selectResult = dbService.ExecuteSelect(query);

                if (selectResult.Count > 0)
                {
                    var firstRow = selectResult[0];
                    int brandId = Convert.ToInt32(firstRow["BrandId"]);
                    string brandName = firstRow["BrandName"].ToString();

                    myTextBlock2.Text = $"Brand: {brandName} (ID: {brandId})";
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
            finally
            {
                dbService.CloseConnection();
            }
        }
    }
}
