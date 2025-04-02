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
using WinUIApp.Models;

namespace WinUIApp.Views.Components.HeaderComponents
{
    public sealed partial class CategorySelectionMenu : UserControl
    {
        public List<Category> Categories { get; set; }

        public CategorySelectionMenu()
        {
            this.InitializeComponent();
            populateCategories();
        }

        private void populateCategories()
        {
            Categories = new List<Category> { new Category (1, "abc") };
        }
    }
}
