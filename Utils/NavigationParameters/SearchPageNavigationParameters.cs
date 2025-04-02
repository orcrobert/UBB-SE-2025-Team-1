using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Models;

namespace WinUIApp.Utils.NavigationParameters
{
    public class SearchPageNavigationParameters
    {
        public List<Category>? InitialCategories { get; set; }
        public string? SearchedTerms { get; set; }
    }
}
