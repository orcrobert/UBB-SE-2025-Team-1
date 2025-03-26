using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MySql.Data.MySqlClient;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        string connectionString = "Server=127.0.0.1;Database=DrinkMDb;User ID=;Password=;Port=3306;";
        
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
            using (var connection = new MySqlConnection(connectionString))
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
