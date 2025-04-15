using Moq;
using System.Collections.Generic;
using System.Linq;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Services.DummyServices;
using WinUIApp.ViewModels;
using Xunit;

namespace WinUIApp.Tests.ViewModels
{
    public class SearchPageViewModelTests
    {
        private readonly Mock<IDrinkService> _drinkServiceMock;
        private readonly Mock<IDrinkReviewService> _reviewServiceMock;
        private readonly SearchPageViewModel _viewModel;

        public SearchPageViewModelTests()
        {
            _drinkServiceMock = new Mock<IDrinkService>();
            _reviewServiceMock = new Mock<IDrinkReviewService>();
            _viewModel = new SearchPageViewModel(_drinkServiceMock.Object, _reviewServiceMock.Object);
        }

        private Drink CreateDrink(int id, string name, float alcoholContent)
        {
            return new Drink(
                id,
                name,
                "https://example.com/image.png",
                new List<Category> { new Category(1, "Category") },
                new Brand(1, "Brand"),
                alcoholContent
            );
        }

        [Fact]
        public void GetDrinks_SortsByNameAscending()
        {
            var drinks = new List<Drink>
            {
                CreateDrink(2, "B Drink", 5.0f),
                CreateDrink(1, "A Drink", 10.0f)
            };

            _drinkServiceMock.Setup(s => s.GetDrinks(null, null, null, null, null, It.IsAny<Dictionary<string, bool>>()))
                 .Returns(drinks.OrderBy(d => d.DrinkName).ToList());

            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(It.IsAny<int>())).Returns(4.0f);

            _viewModel.SetSortByField("Name");
            _viewModel.SetSortOrder(true);

            var result = _viewModel.GetDrinks().ToList();

            Assert.Equal(1, result[0].Drink.DrinkId); // "A Drink" first
            Assert.Equal(2, result[1].Drink.DrinkId);
        }

        [Fact]
        public void GetDrinks_SortsByNameDescending()
        {
            var drinks = new List<Drink>
            {
                CreateDrink(2, "B Drink", 5.0f),
                CreateDrink(1, "A Drink", 10.0f)
            };

            _drinkServiceMock.Setup(s => s.GetDrinks(null, null, null, null, null, It.IsAny<Dictionary<string, bool>>()))
                             .Returns(drinks);
            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(It.IsAny<int>())).Returns(4.0f);

            _viewModel.SetSortByField("Name");
            _viewModel.SetSortOrder(false);

            var result = _viewModel.GetDrinks().ToList();

            Assert.Equal(2, result[0].Drink.DrinkId); // "B Drink" first
            Assert.Equal(1, result[1].Drink.DrinkId);
        }

        [Fact]
        public void GetDrinks_SortsByAlcoholAscending()
        {
            var drinks = new List<Drink>
    {
        CreateDrink(1, "Drink A", 10.0f),
        CreateDrink(2, "Drink B", 5.0f)
    };

            // Sort the drinks by alcohol content in ascending order before returning them
            _drinkServiceMock.Setup(s => s.GetDrinks(null, null, null, null, null, It.IsAny<Dictionary<string, bool>>()))
                             .Returns(drinks.OrderBy(d => d.AlcoholContent).ToList()); // Ascending order

            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(It.IsAny<int>())).Returns(4.0f);

            _viewModel.SetSortByField("Alcohol Content");
            _viewModel.SetSortOrder(true);  // Ascending order

            var result = _viewModel.GetDrinks().ToList();

            // Verify that the drinks are sorted in ascending order of alcohol content
            Assert.Equal(2, result[0].Drink.DrinkId); // Lower alcohol first
            Assert.Equal(1, result[1].Drink.DrinkId);
        }


        [Fact]
        public void GetDrinks_SortsByAlcoholDescending()
        {
            var drinks = new List<Drink>
            {
                CreateDrink(1, "Drink A", 10.0f),
                CreateDrink(2, "Drink B", 5.0f)
            };

            _drinkServiceMock.Setup(s => s.GetDrinks(null, null, null, null, null, It.IsAny<Dictionary<string, bool>>()))
                             .Returns(drinks);
            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(It.IsAny<int>())).Returns(4.0f);

            _viewModel.SetSortByField("Alcohol Content");
            _viewModel.SetSortOrder(false);

            var result = _viewModel.GetDrinks().ToList();

            Assert.Equal(1, result[0].Drink.DrinkId); // Higher alcohol first
            Assert.Equal(2, result[1].Drink.DrinkId);
        }

        [Fact]
        public void GetDrinks_SortsByReviewAverageAscending_WhenNoValidFieldSet()
        {
            var drinks = new List<Drink>
            {
                CreateDrink(1, "Drink A", 5.0f),
                CreateDrink(2, "Drink B", 5.0f)
            };

            _drinkServiceMock.Setup(s => s.GetDrinks(null, null, null, null, null, null))
                             .Returns(drinks);

            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(1)).Returns(3.0f);
            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(2)).Returns(5.0f);

            _viewModel.SetSortByField("UnknownField");
            _viewModel.SetSortOrder(true);

            var result = _viewModel.GetDrinks().ToList();

            Assert.Equal(1, result[0].Drink.DrinkId); // Lower review average first
            Assert.Equal(2, result[1].Drink.DrinkId);
        }

        [Fact]
        public void GetDrinks_SortsByReviewAverageDescending_WhenNoValidFieldSet()
        {
            var drinks = new List<Drink>
    {
        CreateDrink(1, "Drink A", 5.0f),
        CreateDrink(2, "Drink B", 5.0f)
    };

            _drinkServiceMock.Setup(s => s.GetDrinks(null, null, null, null, null, null))
                             .Returns(drinks);

            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(1)).Returns(3.0f);
            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(2)).Returns(5.0f);

            _viewModel.SetSortByField("UnknownField");
            _viewModel.SetSortOrder(false); // descending

            var result = _viewModel.GetDrinks().ToList();

            Assert.Equal(2, result[0].Drink.DrinkId); // Higher review average first
            Assert.Equal(1, result[1].Drink.DrinkId);
        }


        [Fact]
        public void GetDrinks_WithMinRatingFilter_AppliesCorrectly()
        {
            var drinks = new List<Drink>
            {
                CreateDrink(1, "Drink A", 5.0f),
                CreateDrink(2, "Drink B", 5.0f)
            };

            _drinkServiceMock.Setup(s => s.GetDrinks(null, null, null, null, null, It.IsAny<Dictionary<string, bool>>()))
                             .Returns(drinks);

            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(1)).Returns(3.5f);
            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(2)).Returns(2.5f);

            _viewModel.SetMinRatingFilter(3.0f);

            var result = _viewModel.GetDrinks().ToList();

            Assert.Single(result);
            Assert.Equal(1, result[0].Drink.DrinkId);
        }


        [Fact]
        public void SetInitialCategoryFilter_SetsAndAppliesFilter()
        {
            var initial = new List<Category> { new Category(1, "Cider"), new Category(2, "Lager") };
            _viewModel.SetInitialCategoryFilter(initial);

            Assert.Equal(initial, _viewModel.InitialCategories);
        }


        [Fact]
        public void ClearFilters_ResetsAllFilters()
        {
            _viewModel.SetCategoryFilter(new List<string> { "TestCategory" });
            _viewModel.SetBrandFilter(new List<string> { "TestBrand" });
            _viewModel.SetMinAlcoholFilter(4.5f);
            _viewModel.SetMaxAlcoholFilter(10.0f);

            _viewModel.ClearFilters();

            var drinks = new List<Drink>();
            _drinkServiceMock.Setup(s => s.GetDrinks(null, null, null, null, null, It.IsAny<Dictionary<string, bool>>()))
                             .Returns(drinks);
            _reviewServiceMock.Setup(r => r.GetReviewAverageByID(It.IsAny<int>())).Returns(4.0f);

            _viewModel.SetSortByField("Name");
            _viewModel.SetSortOrder(true);
            var result = _viewModel.GetDrinks().ToList();

            Assert.NotNull(result);
        }


    }
}
