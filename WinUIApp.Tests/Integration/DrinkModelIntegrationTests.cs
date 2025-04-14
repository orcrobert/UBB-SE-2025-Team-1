using System;
using System.Collections.Generic;
using Xunit;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.Tests.Integration.Stubs;

namespace WinUIApp.Tests.Integration
{
    public class DrinkModelIntegrationTests : IDisposable
    {
        private readonly StubDrinkModelforDrinkModelIntegration _drinkModel;
        private readonly TestDatabaseHelper _dbHelper;

        public DrinkModelIntegrationTests()
        {
            // Initialize the database service
            var databaseService = DatabaseService.Instance;

            // Initialize the DrinkModel stub
            _drinkModel = new StubDrinkModelforDrinkModelIntegration();

            // Initialize the database helper
            _dbHelper = new TestDatabaseHelper(databaseService);

            // Clear and setup test data
            _dbHelper.ClearDatabase();
            SetupTestData();
        }

        private void SetupTestData()
        {
            // Seed the database with initial test data
            _dbHelper.SeedTestData();
        }

        public void Dispose()
        {
            // Clear the database after tests
            _dbHelper.ClearDatabase();
        }

        [Fact]
        public void GetDrinkById_ValidId_ReturnsDrink()
        {
            // Arrange
            var expectedDrink = _dbHelper.CreateTestDrink();

            // Add the drink to the stub model's collection
            _drinkModel.AddDrink(
                expectedDrink.DrinkName,
                expectedDrink.DrinkImageUrl,
                expectedDrink.CategoryList,
                expectedDrink.DrinkBrand.BrandName,
                expectedDrink.AlcoholContent);

            // Since AddDrink might assign a different ID, get the newly created drink
            var allDrinks = _drinkModel.GetDrinks(expectedDrink.DrinkName, null, null, null, null, null);
            var addedDrink = allDrinks.FirstOrDefault();

            // Act
            var result = _drinkModel.GetDrinkById(addedDrink.DrinkId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(addedDrink.DrinkId, result.DrinkId);
            Assert.Equal(expectedDrink.DrinkName, result.DrinkName);
            Assert.Equal(expectedDrink.DrinkImageUrl, result.DrinkImageUrl);
            Assert.Equal(expectedDrink.AlcoholContent, result.AlcoholContent);
            Assert.Equal(expectedDrink.DrinkBrand.BrandName, result.DrinkBrand.BrandName);
        }


        [Fact]
        public void GetDrinkById_InvalidId_ReturnsNull()
        {
            // Arrange
            int invalidId = -1;

            // Act
            var result = _drinkModel.GetDrinkById(invalidId);

            // Assert
            Assert.Null(result);
        }

        
        [Fact]
        public void GetDrinkById_WithCategories_ReturnsDrinkWithCategories()
        {
            // Arrange
            var testCategories = new List<Category>
    {
        new Category(1, "Test Category 1"),
        new Category(2, "Test Category 2")
    };
            var testDrink = _dbHelper.CreateTestDrinkWithCategories(testCategories);

            // Add the drink to the stub model's collection
            _drinkModel.AddDrink(
                testDrink.DrinkName,
                testDrink.DrinkImageUrl,
                testDrink.CategoryList,
                testDrink.DrinkBrand.BrandName,
                testDrink.AlcoholContent);

            // Get the newly added drink (which might have a different ID)
            var allDrinks = _drinkModel.GetDrinks(testDrink.DrinkName, null, null, null, null, null);
            var addedDrink = allDrinks.FirstOrDefault();

            // Act
            var result = _drinkModel.GetDrinkById(addedDrink.DrinkId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testCategories.Count, result.CategoryList.Count);
            Assert.All(testCategories, category =>
                Assert.Contains(result.CategoryList, c =>
                    c.CategoryId == category.CategoryId &&
                    c.CategoryName == category.CategoryName));
        }

        [Fact]
        public void GetDrinks_NoFilters_ReturnsAllDrinks()
        {
            // Arrange
            // Create and add the test drinks to the model
            for (int i = 0; i < 3; i++)
            {
                var testDrink = _dbHelper.CreateTestDrink(drinkName: $"TestDrink{i}");
                _drinkModel.AddDrink(
                    testDrink.DrinkName,
                    testDrink.DrinkImageUrl,
                    testDrink.CategoryList,
                    testDrink.DrinkBrand.BrandName,
                    testDrink.AlcoholContent);
            }

            // Act
            var result = _drinkModel.GetDrinks(null, null, null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        
        [Fact]
        public void GetDrinks_SearchTerm_ReturnsMatchingDrinks()
        {
            // Arrange
            var searchTerm = "TestDrink";

            // Create and add matching drink
            var matchingDrink = _dbHelper.CreateTestDrink(drinkName: searchTerm);
            _drinkModel.AddDrink(
                matchingDrink.DrinkName,
                matchingDrink.DrinkImageUrl,
                matchingDrink.CategoryList,
                matchingDrink.DrinkBrand.BrandName,
                matchingDrink.AlcoholContent);

            // Create and add non-matching drink
            var otherDrink = _dbHelper.CreateTestDrink(drinkName: "OtherDrink");
            _drinkModel.AddDrink(
                otherDrink.DrinkName,
                otherDrink.DrinkImageUrl,
                otherDrink.CategoryList,
                otherDrink.DrinkBrand.BrandName,
                otherDrink.AlcoholContent);

            // Act
            var result = _drinkModel.GetDrinks(searchTerm, null, null, null, null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchTerm, result[0].DrinkName);
        }

        [Fact]
        public void GetDrinks_BrandFilter_ReturnsOnlyMatchingBrands()
        {
            // Arrange
            var brandName = "TestBrand";

            // Create and add matching drink
            var matchingDrink = _dbHelper.CreateTestDrink(brandName: brandName);
            _drinkModel.AddDrink(
                matchingDrink.DrinkName,
                matchingDrink.DrinkImageUrl,
                matchingDrink.CategoryList,
                matchingDrink.DrinkBrand.BrandName,
                matchingDrink.AlcoholContent);

            // Create and add non-matching drink
            var otherDrink = _dbHelper.CreateTestDrink(brandName: "OtherBrand");
            _drinkModel.AddDrink(
                otherDrink.DrinkName,
                otherDrink.DrinkImageUrl,
                otherDrink.CategoryList,
                otherDrink.DrinkBrand.BrandName,
                otherDrink.AlcoholContent);

            // Act
            var result = _drinkModel.GetDrinks(null, new List<string> { brandName }, null, null, null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal(brandName, result[0].DrinkBrand.BrandName);
        }

        [Fact]
        public void GetDrinks_CategoryFilter_ReturnsOnlyDrinksInCategory()
        {
            // Arrange
            var category = new Category(1, "TestCategory");

            // Create and add a drink with the category
            var matchingDrink = _dbHelper.CreateTestDrinkWithCategories(new List<Category> { category });
            _drinkModel.AddDrink(
                matchingDrink.DrinkName,
                matchingDrink.DrinkImageUrl,
                matchingDrink.CategoryList,
                matchingDrink.DrinkBrand.BrandName,
                matchingDrink.AlcoholContent);

            // Create and add a drink without the category
            var otherDrink = _dbHelper.CreateTestDrink();
            _drinkModel.AddDrink(
                otherDrink.DrinkName,
                otherDrink.DrinkImageUrl,
                otherDrink.CategoryList,
                otherDrink.DrinkBrand.BrandName,
                otherDrink.AlcoholContent);

            // Act
            var result = _drinkModel.GetDrinks(null, null, new List<string> { category.CategoryName }, null, null, null);

            // Assert
            Assert.Single(result);
            Assert.Contains(result[0].CategoryList, c => c.CategoryName == category.CategoryName);
        }

        [Fact]
        public void GetDrinks_AlcoholContentFilter_ReturnsFilteredDrinks()
        {
            // Arrange
            // Create and add a drink with 5.0% alcohol
            var matchingDrink = _dbHelper.CreateTestDrink(alcoholContent: 5.0f);
            _drinkModel.AddDrink(
                matchingDrink.DrinkName,
                matchingDrink.DrinkImageUrl,
                matchingDrink.CategoryList,
                matchingDrink.DrinkBrand.BrandName,
                matchingDrink.AlcoholContent);

            // Create and add a drink with 15.0% alcohol
            var nonMatchingDrink = _dbHelper.CreateTestDrink(alcoholContent: 15.0f);
            _drinkModel.AddDrink(
                nonMatchingDrink.DrinkName,
                nonMatchingDrink.DrinkImageUrl,
                nonMatchingDrink.CategoryList,
                nonMatchingDrink.DrinkBrand.BrandName,
                nonMatchingDrink.AlcoholContent);

            // Act
            var result = _drinkModel.GetDrinks(null, null, null, 4.0f, 6.0f, null);

            // Assert
            Assert.Single(result);
            Assert.True(result[0].AlcoholContent >= 4.0f && result[0].AlcoholContent <= 6.0f);
        }

        [Fact]
        public void AddDrink_NewDrinkWithExistingBrand_DrinkAddedCorrectly()
        {
            // Arrange
            var existingBrand = _dbHelper.CreateTestBrand();
            var categories = new List<Category>
            {
                new Category(1, "Test Category")
            };

            // Act
            _drinkModel.AddDrink("TestDrink", "test.jpg", categories, existingBrand.BrandName, 5.0f);

            // Assert
            var addedDrink = _drinkModel.GetDrinks("TestDrink", null, null, null, null, null)[0];
            Assert.Equal("TestDrink", addedDrink.DrinkName);
            Assert.Equal(existingBrand.BrandName, addedDrink.DrinkBrand.BrandName);
        }

        [Fact]
        public void AddDrink_NewDrinkWithNewBrand_CreatesNewBrandAndDrink()
        {
            // Arrange
            var newBrandName = "NewTestBrand";
            var categories = new List<Category>
            {
                new Category(1, "Test Category")
            };

            // Act
            _drinkModel.AddDrink("TestDrink", "test.jpg", categories, newBrandName, 5.0f);

            // Assert
            var addedDrink = _drinkModel.GetDrinks("TestDrink", null, null, null, null, null)[0];
            Assert.Equal(newBrandName, addedDrink.DrinkBrand.BrandName);
        }

        [Fact]
        public void DeleteDrink_ExistingDrink_RemovesDrinkFromDatabase()
        {
            // Arrange
            var testDrink = _dbHelper.CreateTestDrink();

            // Act
            _drinkModel.DeleteDrink(testDrink.DrinkId);

            // Assert
            var result = _drinkModel.GetDrinkById(testDrink.DrinkId);
            Assert.Null(result);
        }

        [Fact]
        public void DeleteDrink_DrinkWithCategories_RemovesDrinkAndAssociations()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category(1, "Test Category 1"),
                new Category(2, "Test Category 2")
            };
            var testDrink = _dbHelper.CreateTestDrinkWithCategories(categories);

            // Act
            _drinkModel.DeleteDrink(testDrink.DrinkId);

            // Assert
            var result = _drinkModel.GetDrinkById(testDrink.DrinkId);
            Assert.Null(result);
        }

        [Fact]
        public void UpdateDrink_BasicProperties_UpdatesCorrectly()
        {
            // Arrange
            var originalDrink = _dbHelper.CreateTestDrink();

            // Add the drink to the stub model's collection
            _drinkModel.AddDrink(
                originalDrink.DrinkName,
                originalDrink.DrinkImageUrl,
                originalDrink.CategoryList,
                originalDrink.DrinkBrand.BrandName,
                originalDrink.AlcoholContent);

            // Since AddDrink might assign a different ID, get the newly created drink
            var allDrinks = _drinkModel.GetDrinks(originalDrink.DrinkName, null, null, null, null, null);
            var addedDrink = allDrinks.FirstOrDefault();

            var updatedDrink = new Drink(
                addedDrink.DrinkId, // Use the ID from the added drink
                "Updated Name",
                "updated.jpg",
                addedDrink.CategoryList,
                addedDrink.DrinkBrand,
                6.0f
            );

            // Act
            _drinkModel.UpdateDrink(updatedDrink);

            // Assert
            var result = _drinkModel.GetDrinkById(addedDrink.DrinkId);
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.DrinkName);
            Assert.Equal("updated.jpg", result.DrinkImageUrl);
            Assert.Equal(6.0f, result.AlcoholContent);
        }

        [Fact]
        public void GetPersonalDrinkList_UserWithDrinks_ReturnsDrinks()
        {
            // Arrange
            var userId = 1;
            var expectedDrink = _dbHelper.CreateTestDrink();

            // Make sure the drink is actually in the model
            _drinkModel.AddDrink(
                expectedDrink.DrinkName,
                expectedDrink.DrinkImageUrl,
                expectedDrink.CategoryList,
                expectedDrink.DrinkBrand.BrandName,
                expectedDrink.AlcoholContent);

            // Get the newly added drink (which might have a different ID)
            var allDrinks = _drinkModel.GetDrinks(expectedDrink.DrinkName, null, null, null, null, null);
            var addedDrink = allDrinks.FirstOrDefault();

            // Now add this drink to the user's list through the model, not the helper
            _drinkModel.AddToPersonalDrinkList(userId, addedDrink.DrinkId);

            // Act
            var result = _drinkModel.GetPersonalDrinkList(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(addedDrink.DrinkId, result[0].DrinkId);
        }

        [Fact]
        public void GetPersonalDrinkList_UserWithNoDrinks_ReturnsEmptyList()
        {
            // Arrange
            var userId = 999; // User with no drinks

            // Act
            var result = _drinkModel.GetPersonalDrinkList(userId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetDrinkOfTheDay_ExistingValid_ReturnsDrinkOfTheDay()
        {
            // Arrange
            var stubModel = new StubDrinkModelforDrinkModelIntegration();

            // Add a test drink first
            stubModel.AddDrink("Test Drink", "url", new List<Category>(), "Test Brand", 5.0f);
            int drinkId = 1; // The first drink will have ID 1 based on your implementation

            // Vote for this drink (you need at least one user to vote)
            int userId = 1;
            stubModel.VoteDrinkOfTheDay(userId, drinkId);

            // Act
            var result = stubModel.GetDrinkOfTheDay();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(drinkId, result.DrinkId);
        }

        [Fact]
        public void IsDrinkInPersonalList_DrinkInList_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var testDrink = _dbHelper.CreateTestDrink();

            // Make sure the drink is actually in the model
            _drinkModel.AddDrink(
                testDrink.DrinkName,
                testDrink.DrinkImageUrl,
                testDrink.CategoryList,
                testDrink.DrinkBrand.BrandName,
                testDrink.AlcoholContent);

            // Get the newly added drink (which might have a different ID)
            var allDrinks = _drinkModel.GetDrinks(testDrink.DrinkName, null, null, null, null, null);
            var addedDrink = allDrinks.FirstOrDefault();

            // Add to personal list
            _drinkModel.AddToPersonalDrinkList(userId, addedDrink.DrinkId);

            // Act
            var result = _drinkModel.IsDrinkInPersonalList(userId, addedDrink.DrinkId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDrinkInPersonalList_DrinkNotInList_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var testDrink = _dbHelper.CreateTestDrink();

            // Make sure the drink is actually in the model
            _drinkModel.AddDrink(
                testDrink.DrinkName,
                testDrink.DrinkImageUrl,
                testDrink.CategoryList,
                testDrink.DrinkBrand.BrandName,
                testDrink.AlcoholContent);

            // Get the newly added drink (which might have a different ID)
            var allDrinks = _drinkModel.GetDrinks(testDrink.DrinkName, null, null, null, null, null);
            var addedDrink = allDrinks.FirstOrDefault();

            // Don't add to personal list

            // Act
            var result = _drinkModel.IsDrinkInPersonalList(userId, addedDrink.DrinkId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteFromPersonalDrinkList_ExistingDrink_RemovesFromList()
        {
            // Arrange
            var userId = 1;
            var testDrink = _dbHelper.CreateTestDrink();

            // Make sure the drink is actually in the model
            _drinkModel.AddDrink(
                testDrink.DrinkName,
                testDrink.DrinkImageUrl,
                testDrink.CategoryList,
                testDrink.DrinkBrand.BrandName,
                testDrink.AlcoholContent);

            // Get the newly added drink (which might have a different ID)
            var allDrinks = _drinkModel.GetDrinks(testDrink.DrinkName, null, null, null, null, null);
            var addedDrink = allDrinks.FirstOrDefault();

            // Add to personal list first
            _drinkModel.AddToPersonalDrinkList(userId, addedDrink.DrinkId);

            // Act
            var result = _drinkModel.DeleteFromPersonalDrinkList(userId, addedDrink.DrinkId);

            // Assert
            Assert.True(result);
            Assert.False(_drinkModel.IsDrinkInPersonalList(userId, addedDrink.DrinkId));
        }

        [Fact]
        public void DeleteFromPersonalDrinkList_DrinkNotInList_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var testDrink = _dbHelper.CreateTestDrink();

            // Make sure the drink is actually in the model
            _drinkModel.AddDrink(
                testDrink.DrinkName,
                testDrink.DrinkImageUrl,
                testDrink.CategoryList,
                testDrink.DrinkBrand.BrandName,
                testDrink.AlcoholContent);

            // Get the newly added drink (which might have a different ID)
            var allDrinks = _drinkModel.GetDrinks(testDrink.DrinkName, null, null, null, null, null);
            var addedDrink = allDrinks.FirstOrDefault();

            // Don't add to personal list

            // Act
            var result = _drinkModel.DeleteFromPersonalDrinkList(userId, addedDrink.DrinkId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VoteDrinkOfTheDay_NewVote_AddsVote()
        {
            // Arrange
            var userId = 1;
            var testDrink = _dbHelper.CreateTestDrink();

            // Act
            _drinkModel.VoteDrinkOfTheDay(userId, testDrink.DrinkId);

            // Assert
            var votedDrink = _drinkModel.GetCurrentTopVotedDrink();
            Assert.Equal(testDrink.DrinkId, votedDrink);
        }
    }
}
