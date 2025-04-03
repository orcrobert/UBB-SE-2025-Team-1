using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.SearchPageComponents
{
    public sealed partial class RatingFilterComponent : UserControl
    {

        public event EventHandler<float> RatingChanged;

        public RatingFilterComponent()
        {
            this.InitializeComponent();
        }

        private void RatingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RatingComboBox.SelectedItem is ComboBoxItem)
            {
                float stars = 5 - RatingComboBox.SelectedIndex;
                RatingChanged?.Invoke(this, stars);
            }
        }

        public void SetSortBy(string sortField)
        {
            switch (sortField)
            {
                case "5 Stars":
                    RatingComboBox.SelectedIndex = 0;
                    break;
                case "4 Stars":
                    RatingComboBox.SelectedIndex = 1;
                    break;
                case "3 Stars":
                    RatingComboBox.SelectedIndex = 2;
                    break;
                case "2 Stars":
                    RatingComboBox.SelectedIndex = 3;
                    break;
                case "1 Star":
                    RatingComboBox.SelectedIndex = 4;
                    break;
            }
        }

        public void ClearSelection()
        {
            RatingComboBox.SelectedItem = null;
        }
    }
}
