using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.Views.Pages;
using WinUIApp.Views.ViewModels;

namespace WinUIApp.Views
{
    public sealed partial class MainWindow : Window
    {
        public MainPageViewModel ViewModel { get; } = new MainPageViewModel();

        public MainWindow()
        {
            this.InitializeComponent();
            mainGrid = (Grid)Content; // Ensure the Grid is assigned AFTER initialization
            Content = mainGrid;
        }

        private Grid mainGrid; // Add a field for the Grid

        private void GoToDrinkListTest_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DrinkListTestPage), ViewModel);
        }

        // You can keep or remove the SetFixedSize and other commented-out code as needed
        // private void SetFixedSize(int width, int height)
        // {
        //     IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        //     var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        //     var appWindow = AppWindow.GetFromWindowId(windowId);
        //
        //     appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
        // }
    }
}