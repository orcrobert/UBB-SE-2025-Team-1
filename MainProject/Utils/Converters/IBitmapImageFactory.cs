using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace WinUIApp.Utils.Converters
{
    /// <summary>
    /// Provides a factory interface to create BitmapImage instances.
    /// </summary>
    public interface IBitmapImageFactory
    {
        BitmapImage Create(string uri);
    }
}
