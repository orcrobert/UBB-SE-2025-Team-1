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
using WinUIApp.Views.Pages;


namespace WinUIApp.Views.Components
{
    public sealed partial class DrinkComponent : UserControl
    {
        public static readonly DependencyProperty BrandProperty =
            DependencyProperty.Register("Brand", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty AlcoholProperty =
            DependencyProperty.Register("Alcohol", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        public string Brand
        {
            get { return (string)GetValue(BrandProperty); }
            set { SetValue(BrandProperty, value); }
        }

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public string Alcohol
        {
            get { return (string)GetValue(AlcoholProperty); }
            set { SetValue(AlcoholProperty, value); }
        }

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public DrinkComponent()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }


    }
}