using System.Collections.Generic;
using WinUIApp.Models;

namespace WinUIApp.Utils.NavigationParameters
{
    public class SearchPageNavigationParameters
    {
        public List<Category>? InitialCategories { get; set; }
        public string? SearchedTerms { get; set; }
    }
}
