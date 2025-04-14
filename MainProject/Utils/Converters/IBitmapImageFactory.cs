// <copyright file="IBitmapImageFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Utils.Converters
{
    using System;
    using Microsoft.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Provides a factory interface to create BitmapImage instances.
    /// </summary>
    public interface IBitmapImageFactory
    {
        /// <summary>
        /// Creates a BitmapImage from a given URI.
        /// </summary>
        /// <param name="uri"> Uri. </param>
        /// <returns>Bitmap image.</returns>
        BitmapImage Create(string uri);
    }
}