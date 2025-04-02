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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.DrinkOfTheDayComponents
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrinkOfTheDayComponent : UserControl
    {
        public DrinkOfTheDayComponent()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("DrinkName", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty BrandProperty =
            DependencyProperty.Register("DrinkBrand", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("DrinkCategory", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty AlcoholProperty =
            DependencyProperty.Register("AlcoholContent", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

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

        public string DrinkCategory
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public string AlcoholContent
        {
            get { return (string)GetValue(AlcoholProperty); }
            set { SetValue(AlcoholProperty, value); }
        }

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

    }
}
