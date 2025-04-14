// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views
{
    using System;
    using Microsoft.UI;
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;
    using WinUIApp.Views.Pages;

    /// <summary>
    /// MainWindow.xaml's code-behind file.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.SetFixedSize(1440, 900);
            AppMainFrame = this.MainFrame;
            this.MainFrame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Gets the main frame of the application. This is used for navigation between pages.
        /// </summary>
        public static Frame AppMainFrame { get; private set; }

        /// <summary>
        /// Gets or sets the previous page that was navigated to. This is used for navigation back to the previous page.
        /// </summary>
        public static Type PreviousPage { get; set; }

        private void SetFixedSize(int width, int height)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
        }
    }
}