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
using WinUIApp.Views.ViewModels;
using WinUIApp.Utils.NavigationParameters;
using WinUIApp.Views.Pages;

namespace WinUIApp.Views.Components.HeaderComponents
{
    public sealed partial class Header : UserControl
    {
        private HeaderViewModel _viewModel;
        public Header()
        {
            this.InitializeComponent();
            _viewModel = new HeaderViewModel(new Services.DrinkService());
            CategoryMenu.PopulateCategories(_viewModel.GetCategories());
        }

        private void Logo_Click(object sender, RoutedEventArgs e)
        {
            //naviagation to main page
            //MainWindow.AppMainFrame.Navigate();
        }

        private void SearchDrinksButton_Click(object sender, RoutedEventArgs e)
        {
            SearchPageNavigationParameters parameters = new SearchPageNavigationParameters
            {
                InitialCategories = CategoryMenu.SelectedCategories.ToList(),
                SearchedTerms = DrinkSearchBox.Text
            };
            MainWindow.AppMainFrame.Navigate(typeof(SearchPage), parameters);
        }
    }
}
