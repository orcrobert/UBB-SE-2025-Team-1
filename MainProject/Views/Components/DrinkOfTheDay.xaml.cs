// <copyright file="DrinkOfTheDay.xaml.cs" company="PlaceholderCompany">
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
    using WinUIApp.Models;
    using WinUIApp.Utils.Converters;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrinkOfTheDayComponent : UserControl
    {
        private const float DefaultFloatValue = 0.0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkOfTheDayComponent"/> class.
        /// </summary>
        public DrinkOfTheDayComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// DrinkNameProperty is a dependency property that represents the name of the drink.
        /// </summary>
        public static readonly DependencyProperty DrinkNameProperty =
            DependencyProperty.Register("DrinkName", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        /// <summary>
        /// DrinkBrandProperty is a dependency property that represents the brand of the drink.
        /// </summary>
        public static readonly DependencyProperty DrinkBrandProperty =
            DependencyProperty.Register("DrinkBrand", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        /// <summary>
        /// DrinkCategoriesProperty is a dependency property that represents the categories of the drink.
        /// </summary>
        public static readonly DependencyProperty DrinkCategoriesProperty =
            DependencyProperty.Register("DrinkCategories", typeof(List<Category>), typeof(DrinkOfTheDayComponent), new PropertyMetadata(new List<Category>()));

        /// <summary>
        /// AlcoholContentProperty is a dependency property that represents the alcohol content of the drink.
        /// </summary>
        public static readonly DependencyProperty AlcoholContentProperty =
            DependencyProperty.Register("AlcoholContent", typeof(float), typeof(DrinkOfTheDayComponent), new PropertyMetadata(DefaultFloatValue));

        /// <summary>
        /// ImageSourceProperty is a dependency property that represents the image source of the drink.
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the name of the drink. This property is used to display the drink's name in the UI.
        /// </summary>
        public string DrinkName
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brand of the drink. This property is used to display the brand name in the UI.
        /// </summary>
        public string DrinkBrand
        {
            get { return (string)this.GetValue(DrinkBrandProperty); }
            set { this.SetValue(DrinkBrandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the categories of the drink. This property is used to display the drink's categories in the UI.
        /// </summary>
        public List<Category> DrinkCategories
        {
            get { return (List<Category>)this.GetValue(DrinkCategoriesProperty); }
            set { this.SetValue(DrinkCategoriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the alcohol content of the drink. This property is used to display the drink's alcohol content in the UI.
        /// </summary>
        public float AlcoholContent
        {
            get { return (float)this.GetValue(AlcoholContentProperty); }
            set { this.SetValue(AlcoholContentProperty, value); }
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
