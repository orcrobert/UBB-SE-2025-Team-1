using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Models;

namespace WinUIApp.Views
{
    public sealed partial class DrinkModelCrudTest : Page
    {
        private ObservableCollection<Drink> DrinksList { get; set; } = new ObservableCollection<Drink>();

        public DrinkModelCrudTest()
        {
            this.InitializeComponent();
            dataGridDrinks.ItemsSource = DrinksList;
        }

        private async void buttonGetDrinks(object sender, RoutedEventArgs e)
        {
            try
            {
                DrinkModel model = new DrinkModel();
                List<Drink> drinks = model.getDrinks();

                DrinksList.Clear();
                foreach (Drink drink in drinks)
                {
                    DrinksList.Add(drink);
                }
            }
            catch (Exception ex)
            {
                // Show error message
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }
    }
}
