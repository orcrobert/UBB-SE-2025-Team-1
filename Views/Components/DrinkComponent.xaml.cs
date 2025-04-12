// <copyright file="DrinkComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Navigation;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using WinUIApp.Views.Pages;

    /// <summary>
    /// A user control that represents a drink component in the application.
    /// </summary>
    public sealed partial class DrinkComponent : UserControl
    {
        /// <summary>
        /// BrandProperty is a dependency property that represents the brand of the drink.
        /// </summary>
        public static readonly DependencyProperty BrandProperty =
            DependencyProperty.Register("Brand", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        /// <summary>
        /// CategoryProperty is a dependency property that represents the category of the drink.
        /// </summary>
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        /// <summary>
        /// AlcoholProperty is a dependency property that represents the alcohol content of the drink.
        /// </summary>
        public static readonly DependencyProperty AlcoholProperty =
            DependencyProperty.Register("Alcohol", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        /// <summary>
        /// ImageSourceProperty is a dependency property that represents the image source of the drink.
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkComponent"/> class.
        /// </summary>
        public DrinkComponent()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the brand of the drink. This property is used to display the brand name in the UI.
        /// </summary>
        public string Brand
        {
            get { return (string)this.GetValue(BrandProperty); }
            set { this.SetValue(BrandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the category of the drink. This property is used to display the category name in the UI.
        /// </summary>
        public string Category
        {
            get { return (string)this.GetValue(CategoryProperty); }
            set { this.SetValue(CategoryProperty, value); }
        }

        /// <summary>
        /// Gets or sets the alcohol content of the drink. This property is used to display the alcohol content in the UI.
        /// </summary>
        public string Alcohol
        {
            get { return (string)this.GetValue(AlcoholProperty); }
            set { this.SetValue(AlcoholProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image source of the drink. This property is used to display the drink's image in the UI.
        /// </summary>
        public string ImageSource
        {
            get { return (string)this.GetValue(ImageSourceProperty); }
            set { this.SetValue(ImageSourceProperty, value); }
        }
    }
}