using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using WinUIApp.Models;
using WinUIApp.Services;

namespace WinUIApp.Tests.Integration
{
    public class TestDatabaseHelper
    {
        private readonly DatabaseService _databaseService;

        public TestDatabaseHelper(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void ClearDatabase()
        {
            const string query = @"
                DELETE FROM DrinkCategory;
                DELETE FROM Drink;
                DELETE FROM Brand;
                DELETE FROM Category;
                DELETE FROM UserDrink;
                DELETE FROM Vote;
                DELETE FROM DrinkOfTheDay;";
            _databaseService.ExecuteDataModificationQuery(query);
        }

        public void SeedTestData()
        {
            var brand = CreateTestBrand();
            var categories = new List<Category>
            {
                new Category(1, "Beer"),
                new Category(2, "Wine")
            };
            CreateTestDrinkWithCategories(categories);
        }

        public Drink CreateTestDrink(string drinkName = "TestDrink", string brandName = "TestBrand", float alcoholContent = 5.0f)
        {
            var brand = CreateTestBrand(brandName);

            // Fix: Make sure we properly retrieve the DrinkId by using ExecuteScalar
            using (var connection = new SqlConnection("Server=DESKTOP-UHDRE10\\SQLEXPRESS;Initial Catalog=WinUIApp;Integrated Security=True;TrustServerCertificate=True"))
            {
                connection.Open();

                string query = @"
                    INSERT INTO Drink (DrinkName, DrinkURL, AlcoholContent, BrandId)
                    OUTPUT INSERTED.DrinkId
                    VALUES (@DrinkName, @DrinkURL, @AlcoholContent, @BrandId);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DrinkName", drinkName);
                    command.Parameters.AddWithValue("@DrinkURL", "test.jpg");
                    command.Parameters.AddWithValue("@AlcoholContent", alcoholContent);
                    command.Parameters.AddWithValue("@BrandId", brand.BrandId);

                    // Execute and get the ID directly
                    var result = command.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        throw new Exception($"Failed to create drink '{drinkName}'");
                    }

                    int drinkId = Convert.ToInt32(result);
                    return new Drink(drinkId, drinkName, "test.jpg", new List<Category>(), brand, alcoholContent);
                }
            }
        }

        public Drink CreateTestDrinkWithCategories(List<Category> categories)
        {
            var drink = CreateTestDrink();
            foreach (var category in categories)
            {
                const string query = @"
                    INSERT INTO DrinkCategory (DrinkId, CategoryId)
                    VALUES (@DrinkId, @CategoryId);";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@DrinkId", drink.DrinkId),
                    new SqlParameter("@CategoryId", category.CategoryId)
                };

                _databaseService.ExecuteDataModificationQuery(query, parameters);
            }

            drink.CategoryList = categories;
            return drink;
        }

        public List<Drink> CreateMultipleTestDrinks(int count)
        {
            var drinks = new List<Drink>();
            for (int i = 0; i < count; i++)
            {
                drinks.Add(CreateTestDrink($"TestDrink{i + 1}"));
            }
            return drinks;
        }

        public Brand CreateTestBrand(string brandName = "TestBrand")
        {
            try
            {
                // First try to insert the brand directly, with error handling
                using (var connection = new SqlConnection("Server=DESKTOP-UHDRE10\\SQLEXPRESS;Initial Catalog=WinUIApp;Integrated Security=True;TrustServerCertificate=True"))
                {
                    connection.Open();

                    // Try to insert and get the ID in one operation
                    string insertQuery = @"
            BEGIN TRY
                INSERT INTO Brand (BrandName)
                VALUES (@BrandName);
                SELECT SCOPE_IDENTITY() AS BrandId;
            END TRY
            BEGIN CATCH
                -- If there's a unique constraint violation, just get the existing ID
                IF ERROR_NUMBER() = 2627 OR ERROR_NUMBER() = 2601
                    SELECT BrandId FROM Brand WHERE BrandName = @BrandName;
                ELSE
                    THROW;
            END CATCH";

                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BrandName", brandName);
                        var result = command.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            throw new Exception($"Failed to get brand ID for '{brandName}'");
                        }

                        int brandId = Convert.ToInt32(result);
                        return new Brand(brandId, brandName);
                    }
                }
            }
            catch (SqlException ex)
            {
                // If we still get a unique constraint violation, try to retrieve the brand directly
                if (ex.Number == 2627 || ex.Number == 2601)  // Unique constraint violation error codes
                {
                    using (var connection = new SqlConnection("Server=DESKTOP-UHDRE10\\SQLEXPRESS;Initial Catalog=WinUIApp;Integrated Security=True;TrustServerCertificate=True"))
                    {
                        connection.Open();
                        string query = "SELECT BrandId FROM Brand WHERE BrandName = @BrandName";
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@BrandName", brandName);
                            var result = command.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                int brandId = Convert.ToInt32(result);
                                return new Brand(brandId, brandName);
                            }
                        }
                    }
                }
                throw; // Re-throw if we couldn't handle it
            }

            throw new Exception($"Failed to create or retrieve brand '{brandName}'");
        }

        public void AddDrinkToUserList(int userId, int drinkId)
        {
            const string query = @"
                INSERT INTO UserDrink (UserId, DrinkId)
                VALUES (@UserId, @DrinkId);";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@DrinkId", drinkId)
            };

            _databaseService.ExecuteDataModificationQuery(query, parameters);
        }

        public void SetDrinkOfTheDay(int drinkId)
        {
            const string query = @"
                INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime)
                VALUES (@DrinkId, GETDATE());";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DrinkId", drinkId)
            };

            _databaseService.ExecuteDataModificationQuery(query, parameters);
        }

        public void AddVote(int userId, int drinkId)
        {
            const string query = @"
                INSERT INTO Vote (UserId, DrinkId, VoteTime)
                VALUES (@UserId, @DrinkId, GETDATE());";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@DrinkId", drinkId)
            };

            _databaseService.ExecuteDataModificationQuery(query, parameters);
        }
    }
}