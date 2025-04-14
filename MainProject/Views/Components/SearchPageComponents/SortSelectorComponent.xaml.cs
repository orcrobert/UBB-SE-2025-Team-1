// <copyright file="SortSelectorComponent.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// User control for selecting sorting options in a search page.
    /// </summary>
    public sealed partial class SortSelectorComponent : UserControl
    {
        private const string SortFieldName = "Name";
        private const string SortFieldAlcoholContent = "Alcohol Content";
        private const string SortFieldAverageReviewScore = "Average Review Score";
        private const string SortOrderAscending = "Ascending";
        private const int SortByNameIndex = 0;
        private const int SortByAlcoholContentIndex = 1;
        private const int SortByAverageReviewScoreIndex = 2;
        private const int SortOrderAscendingIndex = 0;
        private const int SortOrderDescendingIndex = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortSelectorComponent"/> class.
        /// </summary>
        public SortSelectorComponent()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event that fires when the sort order changes, providing a boolean indicating if the order is ascending.
        /// </summary>
        public event EventHandler<bool> SortOrderChanged;

        /// <summary>
        /// Event that fires when the sort field changes, providing the selected sort field name.
        /// </summary>
        public event EventHandler<string> SortByChanged;

        /// <summary>
        /// Sets the sort order selector based on the specified direction.
        /// </summary>
        /// <param name="isAscending">True for ascending order, false for descending.</param>
        public void SetSortOrder(bool isAscending)
        {
            this.SortOrderComboBox.SelectedIndex = isAscending ? SortOrderAscendingIndex : SortOrderDescendingIndex;
        }

        /// <summary>
        /// Sets the sort field selector to the specified field.
        /// </summary>
        /// <param name="sortField">The sort field to select.</param>
        public void SetSortBy(string sortField)
        {
            switch (sortField)
            {
                case SortFieldName:
                    this.SortByComboBox.SelectedIndex = SortByNameIndex;
                    break;
                case SortFieldAlcoholContent:
                    this.SortByComboBox.SelectedIndex = SortByAlcoholContentIndex;
                    break;
                case SortFieldAverageReviewScore:
                    this.SortByComboBox.SelectedIndex = SortByAverageReviewScoreIndex;
                    break;
            }
        }

        /// <summary>
        /// Handles selection changes in the sort field combo box and triggers the SortByChanged event
        /// with the selected sort field name.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="selectionChangedEventArgs">Event data for the selection changed event.</param>
        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (this.SortByComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortField = selectedItem.Content.ToString();
                this.SortByChanged?.Invoke(this, sortField);
            }
        }

        /// <summary>
        /// Handles selection changes in the sort order combo box and triggers the SortOrderChanged event
        /// with a boolean indicating if the order is ascending.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="selectionChangedEventArgs">Event data for the selection changed event.</param>
        private void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (this.SortOrderComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                bool isAscending = selectedItem.Content.ToString() == SortOrderAscending;
                this.SortOrderChanged?.Invoke(this, isAscending);
            }
        }
    }
}
