using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace WinUIApp.Utils.Converters
{
    /// <summary>
    /// The default implementation that creates BitmapImage instances.
    /// </summary>
    public class DefaultBitmapImageFactory : IBitmapImageFactory
    {
        public BitmapImage Create(string uri)
        {
            return new BitmapImage(new Uri(uri));
        }
    }
}
