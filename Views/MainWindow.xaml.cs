using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Services;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WinUIApp.Services.DummyServies;
using WinUIApp.Models;

namespace WinUIApp.Views
{
    public sealed partial class MainWindow : Window
    {
        public ObservableCollection<Drink> DrinksList { get; set; } = new ObservableCollection<Drink>();

        public MainWindow()
        {
            this.InitializeComponent();
            LoadDrinksFromDatabase();
        }

        public void LoadDrinksFromDatabase()
        {
            var drinkModel = new DrinkModel();
            var drinks = drinkModel.getDrinks(null, null, null, null, null, null);

            Debug.WriteLine($"Retrieved {drinks.Count} drinks from the database.");
            DrinksList.Clear();
            foreach (var drink in drinks)
            {
                Debug.WriteLine($"Adding drink: {drink.DrinkName}");
                DrinksList.Add(drink);
            }
        }

        private void AddDrinkButton_Click(object sender, RoutedEventArgs e)
        {
            var addDrinkWindow = new NewWindow(this);
            addDrinkWindow.Activate();
        }
    }
}
