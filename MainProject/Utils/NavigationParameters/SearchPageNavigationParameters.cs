using System.Collections.Generic;
using WinUIApp.Models;

namespace WinUIApp.Utils.NavigationParameters
{
    public class SearchPageNavigationParameters
    {
        public List<Category>? SelectedCategoryFilters { get; set; }
        public string? InputSearchKeyword { get; set; }
    }
}
