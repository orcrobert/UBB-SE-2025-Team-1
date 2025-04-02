using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace WinUIApp.Views
{
    public sealed partial class MainWindow : Window
    {
        public static Frame AppMainFrame { get; private set; }

        public MainWindow()
        {
            this.InitializeComponent();
            SetFixedSize(1440, 900);
            AppMainFrame = MainFrame;

            //navigation to main page
            //MainFrame.Navigate();
            DrinkService drinkService = new DrinkService();
            ReviewService reviewService = new ReviewService();
            UserService userService = new UserService();
            MainPageViewModel mainPageViewModel = new MainPageViewModel();
            MainPage mainPage = new MainPage();
            this.Content = mainPage;
        }

        private void SetFixedSize(int width, int height)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
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