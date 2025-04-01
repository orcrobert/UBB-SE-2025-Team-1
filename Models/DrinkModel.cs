﻿using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Services;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace WinUIApp.Models
{
    class DrinkModel
    {
        public List<Drink> getDrinks(string? searchedTerm, List<string>? brandNameFilter, List<string>? categoryFilter, float? minAlcohol, float? maxAlcohol, Dictionary<string, bool>? orderBy)
        {
            var dbService = DatabaseService.Instance;
            List<Drink> drinks = [];

            try
            {
                string getDrinksQuery = @" SELECT D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL,
                                          B.BrandId, B.BrandName, 
                                          GROUP_CONCAT(C.CategoryId ORDER BY C.CategoryId) AS CategoryIds, 
                                          GROUP_CONCAT(C.CategoryName ORDER BY C.CategoryId) AS CategoryNames
                                          FROM Drink AS D
                                          INNER JOIN Brand AS B ON D.BrandId = B.BrandId
                                          LEFT JOIN DrinkCategory AS DC ON D.DrinkId = DC.DrinkId
                                          LEFT JOIN Category AS C ON DC.CategoryId = C.CategoryId
                                          GROUP BY D.DrinkId, B.BrandId";

                List<string> queryParts = [getDrinksQuery];
                List<string> queryConditions = [];
                List<MySqlParameter> queryParameters = [];

                if (!string.IsNullOrEmpty(searchedTerm))
                {
                    List<string> searchTerms = searchedTerm.Split(' ').ToList();

                    for (int i = 0; i < searchTerms.Count; i++)
                    {
                        string parameterName = $"@SearchTerm{i}";
                        queryConditions.Add($"(B.BrandName LIKE {parameterName} OR C.CategoryName LIKE {parameterName} OR D.DrinkName LIKE {parameterName})");

                        queryParameters.Add(new MySqlParameter(parameterName, MySqlDbType.VarChar) { Value = "%" + searchTerms[i] + "%" });
                    }
                }

                if (brandNameFilter != null && brandNameFilter.Count != 0)
                {
                    var brandParams = brandNameFilter.Select((b, i) => $"@Brand{i}").ToList();
                    queryConditions.Add($"B.BrandName IN ({string.Join(", ", brandParams)})");

                    for (int i = 0; i < brandNameFilter.Count; i++)
                    {
                        queryParameters.Add(new MySqlParameter($"@Brand{i}", MySqlDbType.VarChar) { Value = brandNameFilter[i] });
                    }
                }

                if (categoryFilter != null && categoryFilter.Count != 0)
                {
                    var categoryParams = categoryFilter.Select((c, i) => $"@Category{i}").ToList();
                    queryConditions.Add($"C.CategoryName IN ({string.Join(", ", categoryParams)})");

                    for (int i = 0; i < categoryFilter.Count; i++)
                    {
                        queryParameters.Add(new MySqlParameter($"@Category{i}", MySqlDbType.VarChar) { Value = categoryFilter[i] });
                    }
                }

                if (minAlcohol.HasValue)
                {
                    queryConditions.Add("D.AlcoholContent >= @MinAlcohol");
                    queryParameters.Add(new MySqlParameter("@MinAlcohol", MySqlDbType.Float) { Value = minAlcohol.Value });
                }

                if (maxAlcohol.HasValue)
                {
                    queryConditions.Add("D.AlcoholContent <= @MaxAlcohol");
                    queryParameters.Add(new MySqlParameter("@MaxAlcohol", MySqlDbType.Float) { Value = maxAlcohol.Value });
                }

                if (queryConditions.Any())
                {
                    queryParts.Add("WHERE " + string.Join(" AND ", queryConditions));
                }
                
                if (orderBy != null && orderBy.Count != 0)
                {
                    var orderClauses = orderBy.Select(o => $"{o.Key} {(o.Value ? "ASC" : "DESC")}");
                    queryParts.Add("ORDER BY " + string.Join(", ", orderClauses));
                }

                var drinkQueryResult = dbService.ExecuteSelect(string.Join(" ", queryParts), queryParameters);

                foreach (var row in drinkQueryResult)
                {
                    int drinkId = Convert.ToInt32(row["DrinkId"]);
                    float alcoholContent = Convert.ToSingle(row["AlcoholContent"]);
                    string drinkName = row["DrinkName"].ToString();
                    string drinkURL = row["DrinkURL"].ToString();

                    int brandId = Convert.ToInt32(row["BrandId"]);
                    string brandName = row["BrandName"].ToString();
                    Brand brand = new Brand(brandId, brandName);

                    List<Category> categories = [];

                    if (row["CategoryIds"] != DBNull.Value && row["CategoryNames"] != DBNull.Value)
                    {
                        string[] categoryIds = row["CategoryIds"].ToString().Split(',');
                        string[] categoryNames = row["CategoryNames"].ToString().Split(',');

                        for (int i = 0; i < categoryIds.Length; i++)
                        {
                            int categoryId = Convert.ToInt32(categoryIds[i]);
                            string categoryName = categoryNames[i];
                            categories.Add(new Category(categoryId, categoryName));
                        }
                    }

                    Drink drink = new Drink(drinkId, drinkName, drinkURL, categories, brand, alcoholContent);
                    drinks.Add(drink);
                }

                return drinks;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }



        public void addDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string brandIdQuery = @"SELECT BrandId 
                                        FROM Brand 
                                        WHERE BrandName = @BrandName";

                List<MySqlParameter> brandNameParameter =
                [
                    new MySqlParameter("@BrandName", MySqlDbType.VarChar) { Value = brandName }
                ];
                var brandIdResult = dbService.ExecuteSelect(brandIdQuery, brandNameParameter);
                int brandId = brandIdResult.Count > 0 ? Convert.ToInt32(brandIdResult[0]["BrandId"]) : -1;


                if (brandId == -1)
                {
                    throw new Exception("Brand does not exist");
                }

                string addDrinkQuery = @"INSERT INTO Drink (DrinkName, DrinkUrl, AlcoholContent, BrandId) 
                                VALUES (@DrinkName, @DrinkUrl, @AlcoholContent, @BrandId);";

                List<MySqlParameter> drinkParameters =
                [
                    new MySqlParameter("@DrinkName", MySqlDbType.VarChar) { Value = drinkName },
                    new MySqlParameter("@DrinkUrl", MySqlDbType.VarChar) { Value = drinkUrl },
                    new MySqlParameter("@AlcoholContent", MySqlDbType.Float) { Value = (decimal) alcoholContent },//
                    new MySqlParameter("@BrandId", MySqlDbType.Int32) { Value = brandId }
                ];

                dbService.ExecuteQuery(addDrinkQuery, drinkParameters);


                string lastDrinkIdQuery = "SELECT LAST_INSERT_ID() AS DrinkId";
                var lastDrinkIdResult = dbService.ExecuteSelect(lastDrinkIdQuery);
                int drinkId = Convert.ToInt32(lastDrinkIdResult[0]["DrinkId"]);

                foreach (Category category in categories)
                {
                    string addCategoriesQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) 
                                               VALUES (@DrinkId, @CategoryId);";
                    List<MySqlParameter> categoryParameters =
                    [
                        new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId },
                        new MySqlParameter("@CategoryId", MySqlDbType.Int32) { Value = category.Id }
                    ];

                    dbService.ExecuteQuery(addCategoriesQuery, categoryParameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }

        public void deleteDrink(int drinkId)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string deleteDrinkQuery = @"DELETE FROM Drink WHERE DrinkId = @DrinkId";
                List<MySqlParameter> drinkIdParameter =
                [
                    new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId }
                ];
                dbService.ExecuteQuery(deleteDrinkQuery, drinkIdParameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }

        public void updateDrink(Drink drink)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string brandIdQuery = @"SELECT BrandId FROM Brand 
                                        WHERE BrandName = @BrandName";
                List<MySqlParameter> brandNameParameter =
                [
                    new MySqlParameter("@BrandName", MySqlDbType.VarChar) { Value = drink.Brand.Name }
                ];
                var brandIdResult = dbService.ExecuteSelect(brandIdQuery, brandNameParameter);
                int brandId = brandIdResult.Count > 0 ? Convert.ToInt32(brandIdResult[0]["BrandId"]) : -1;
                if (brandId == -1)
                {
                    throw new Exception("Brand does not exist");
                }


                string updateDrinkQuery = @"UPDATE Drink SET AlcoholContent = @AlcoholContent, BrandId = @BrandId, DrinkName = @DrinkName, DrinkUrl = @DrinkUrl
                                            WHERE DrinkId = @DrinkId;";
                List<MySqlParameter> drinkParameters =
                [
                    new MySqlParameter("@DrinkName", MySqlDbType.VarChar) { Value = drink.DrinkName },
                    new MySqlParameter("@DrinkUrl", MySqlDbType.VarChar) { Value = drink.DrinkURL },
                    new MySqlParameter("@AlcoholContent", MySqlDbType.Float) { Value = drink.AlcoholContent },
                    new MySqlParameter("@BrandId", MySqlDbType.Int32) { Value = brandId },
                    new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drink.Id }
                ];
                dbService.ExecuteQuery(updateDrinkQuery, drinkParameters);


                string getExistingCategoriesQuery = @"SELECT CategoryId FROM DrinkCategory 
                                                    WHERE DrinkId = @DrinkId";
                var existingCategoriesResult = dbService.ExecuteSelect(getExistingCategoriesQuery, drinkParameters);
                HashSet<int> existingCategories = [.. existingCategoriesResult.Select(row => Convert.ToInt32(row["CategoryId"]))];
                HashSet<int> newCategories = [.. drink.Categories.Select(c => c.Id)];

                var categoriesToInsert = newCategories.Except(existingCategories).ToList();
                var categoriesToDelete = existingCategories.Except(newCategories).ToList();

                foreach (int categoryId in categoriesToInsert)
                {
                    string addCategoriesQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) 
                                               VALUES (@DrinkId, @CategoryId);";
                    List<MySqlParameter> categoryParameters =
                    [
                        new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drink.Id },
                        new MySqlParameter("@CategoryId", MySqlDbType.Int32) { Value = categoryId }
                    ];
                    dbService.ExecuteQuery(addCategoriesQuery, categoryParameters);
                }

                foreach (int categoryId in categoriesToDelete)
                {
                    string deleteCategoriesQuery = @"DELETE FROM DrinkCategory 
                                                 WHERE DrinkId = @DrinkId AND CategoryId = @CategoryId";
                    List<MySqlParameter> categoryParameters =
                    [
                        new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drink.Id },
                        new MySqlParameter("@CategoryId", MySqlDbType.Int32) { Value = categoryId }
                    ];
                    dbService.ExecuteQuery(deleteCategoriesQuery, categoryParameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }

        public List<Category> getDrinkCategories()
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string getCategoriesQuery = "SELECT * FROM Category ORDER BY CategoryId;";
                var selectResult = dbService.ExecuteSelect(getCategoriesQuery);

                List<Category> categories = [];
                foreach (var row in selectResult)
                {
                    int categoryId = Convert.ToInt32(row["CategoryId"]);
                    string categoryName = row["CategoryName"].ToString();
                    categories.Add(new Category(categoryId, categoryName));
                }
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }

        public List<Brand> getDrinkBrands()
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string getBrandsQuery = "SELECT * FROM Brand ORDER BY BrandId;";
                var selectResult = dbService.ExecuteSelect(getBrandsQuery);
                List<Brand> brands = [];
                foreach (var row in selectResult)
                {
                    int brandId = Convert.ToInt32(row["BrandId"]);
                    string brandName = row["BrandName"].ToString();
                    brands.Add(new Brand(brandId, brandName));
                }
                return brands;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }

        public List<Drink> getPersonalDrinkList(int userId, int numberOfDrinks = 1)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string getPersonalDrinkList = "SELECT d.DrinkId, d.AlcoholContent, " +
                            "b.BrandId, b.BrandName, " +
                            "GROUP_CONCAT(c.CategoryId ORDER BY c.CategoryId) AS CategoryIds, " +
                            "GROUP_CONCAT(c.CategoryName ORDER BY c.CategoryId) AS CategoryNames " +
                            "FROM UserDrink ud " +
                            "JOIN Drink d ON ud.DrinkId = d.DrinkId " +
                            "JOIN Brand b ON d.BrandId = b.BrandId " +
                            "LEFT JOIN DrinkCategory dc ON d.DrinkId = dc.DrinkId " +
                            "LEFT JOIN Category c ON dc.CategoryId = c.CategoryId " +
                            "WHERE ud.UserId = @UserId " +
                            "GROUP BY d.DrinkId, b.BrandId " +
                            "ORDER BY d.DrinkId " +
                            "LIMIT @NumbersOfDrinks;";


                List<MySqlParameter> parameters =
                [
                    new MySqlParameter("@UserId", MySqlDbType.Int32) { Value = userId },
                    new MySqlParameter("@NumbersOfDrinks", MySqlDbType.Int32) { Value = numberOfDrinks },
                ];

                var selectResult = dbService.ExecuteSelect(getPersonalDrinkList, parameters);
                List<Drink> drinks = [];

                foreach (var row in selectResult)
                {
                    int drinkId = Convert.ToInt32(row["DrinkId"]);
                    float alcoholContent = (float)(row["AlcoholContent"]);
                    string drinkName = row["DrinkName"].ToString();
                    string drinkURL = row["DrinkURL"].ToString();

                    int brandId = Convert.ToInt32(row["BrandId"]);
                    string brandName = row["BrandName"].ToString();
                    Brand brand = new Brand(brandId, brandName);

                    List<Category> categories = [];
                    if (row["CategoryIds"] != DBNull.Value && row["CategoryNames"] != DBNull.Value)
                    {
                        string[] categoryIds = row["CategoryIds"].ToString().Split(',');
                        string[] categoryNames = row["CategoryNames"].ToString().Split(',');

                        for (int i = 0; i < categoryIds.Length; i++)
                        {
                            categories.Add(new Category(Convert.ToInt32(categoryIds[i]), categoryNames[i]));
                        }
                    }

                    Drink drink = new Drink(drinkId, drinkName, drinkURL, categories, brand, alcoholContent);
                    drinks.Add(drink);
                }
                return drinks;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }

        public bool addToPersonalDrinkList(int userId, int drinkId)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string insertQuery = "INSERT INTO UserDrink (UserId, DrinkId) VALUES (@UserId, @DrinkId);";

                List<MySqlParameter> parameters =
                [
                    new MySqlParameter("@UserId", MySqlDbType.Int32) { Value = userId },
                    new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId }
                ];

                int rowsAffected = dbService.ExecuteQuery(insertQuery, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add drink to personal list", ex);
            }
        }

        public bool deleteFromPersonalDrinkList(int userId, int drinkId)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string deleteQuery = "DELETE FROM UserDrink WHERE UserId = @UserId AND DrinkId = @DrinkId;";

                List<MySqlParameter> parameters =
                [
                    new MySqlParameter("@UserId", MySqlDbType.Int32) { Value = userId },
                    new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId }
                ];

                int rowsAffected = dbService.ExecuteQuery(deleteQuery, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete drink from personal list", ex);
            }
        }
    }
}
