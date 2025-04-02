using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using WinUIApp.Services;
using WinUIApp.Services.DummyServies;
using WinUIApp.Views.ModelViews;
using WinUIApp.Views.Pages;

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