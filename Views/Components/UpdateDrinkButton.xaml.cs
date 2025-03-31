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
using Microsoft.UI.Text;
using Microsoft.UI;

namespace WinUIApp.Views.Components
{
    public sealed partial class UpdateDrinkButton : UserControl
    {
        public UpdateDrinkButton()
        {
            this.InitializeComponent();
        }

        private void UpdateDrinkButton_Click(object sender, RoutedEventArgs e)
        {
            var flyout = new Flyout
            {
                Content = new UpdateDrinkFlyout()
            };

            flyout.ShowAt(UpdateButton);
        }

    }
}
