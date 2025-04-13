using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WinUIApp.Models;

namespace WinUIApp.ViewModels
{
    public interface IAddDrinkMenuViewModel
    {
        string AlcoholContent { get; set; }
        List<Brand> AllBrands { get; set; }
        List<string> AllCategories { get; set; }
        List<Category> AllCategoryObjects { get; set; }
        string BrandName { get; set; }
        string DrinkName { get; set; }
        string DrinkURL { get; set; }
        ObservableCollection<string> SelectedCategoryNames { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        void ClearForm();
        List<Category> GetSelectedCategories();
        void InstantAddDrink();
        void SendAddDrinkRequest();
        void ValidateUserDrinkInput();
    }
}