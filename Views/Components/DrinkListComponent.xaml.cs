using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.ViewModels;

namespace WinUIApp.Views.Components
{
    public sealed partial class DrinkListComponent : UserControl
    {
        public static readonly DependencyProperty DrinksProperty =
           DependencyProperty.Register(
               "Drinks",
               typeof(List<Drink>), 
               typeof(DrinkListComponent),
               new PropertyMetadata(null));

        public List<Drink> Drinks
        {
            get => (List<Drink>)GetValue(DrinksProperty);
            set => SetValue(DrinksProperty, value);
        }

        public DrinkListComponent()
        {
            InitializeComponent();
        }
    }
}