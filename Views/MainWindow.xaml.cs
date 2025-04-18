using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.Views.Pages;
namespace WinUIApp.Views
{
    public sealed partial class MainWindow : Window
    {
        public static Frame AppMainFrame { get; private set; }

        public static Type PreviousPage { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();
            SetFixedSize(1440, 900);
            AppMainFrame = MainFrame;

            DrinkService drinkService = new DrinkService();
            ReviewService reviewService = new ReviewService();
            UserService userService = new UserService();
            MainPage mainPage = new MainPage();

            MainFrame.Navigate(typeof(MainPage));
        }

        private void SetFixedSize(int width, int height)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
        }

    }
}