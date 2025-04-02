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
using WinUIApp.Utils.Converters;
using WinUIApp.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrinkOfTheDayComponent : UserControl
    {
        public DrinkOfTheDayComponent()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty DrinkNameProperty =
            DependencyProperty.Register("DrinkName", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty BrandProperty =
            DependencyProperty.Register("DrinkBrand", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("DrinkCategory", typeof(List<Category>), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty AlcoholProperty =
            DependencyProperty.Register("AlcoholContent", typeof(float), typeof(DrinkOfTheDayComponent), new PropertyMetadata(0.0));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        public string DrinkName
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public string DrinkBrand
        {
            get { return (string)GetValue(BrandProperty); }
            set { SetValue(BrandProperty, value); }
        }

        public List<Category> DrinkCategory
        {
            get { return (List<Category>)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public float AlcoholContent
        {
            get { return (float)GetValue(AlcoholProperty); }
            set { SetValue(AlcoholProperty, value); }
        }

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }


    }
}
