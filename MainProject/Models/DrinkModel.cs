// <copyright file="DrinkModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.Data.SqlClient;
    using WinUIApp.Services;

    /// <summary>
    /// Handles all database operations related to drinks.
    /// </summary>
    internal class DrinkModel : IDrinkModel
    {
        private const int NoResultFound = -1;
        private const int ZeroResults = 0;
        private const int FirstResult = 0;

        private readonly DatabaseService dataBaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkModel"/> class.
        /// </summary>
        public DrinkModel()
        {
            this.dataBaseService = DatabaseService.Instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkModel"/> class with a specific database service.
        /// </summary>
        /// <param name="databaseService"> The database service. </param>
        public DrinkModel(DatabaseService databaseService)
        {
            this.dataBaseService = databaseService;
        }

        /// <summary>
        /// Retrieves a drink by its ID from the database.
        /// </summary>
        /// <param name="drinkId"> The drink id. </param>
        /// <returns>The drink, if found. </returns>
        /// <exception cref="Exception"> Any issues that might happen. </exception>
        public Drink? GetDrinkById(int drinkId)
        {
            try
            {
                const string getDrinkQuery = @"
                    SELECT D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL,
                           B.BrandId, B.BrandName,
                           STRING_AGG(C.CategoryId, ',') WITHIN GROUP (ORDER BY C.CategoryId) AS CategoryIds,
                           STRING_AGG(C.CategoryName, ',') WITHIN GROUP (ORDER BY C.CategoryId) AS CategoryNames
                    FROM Drink AS D
                    LEFT JOIN Brand AS B ON D.BrandId = B.BrandId
                    LEFT JOIN DrinkCategory AS DC ON D.DrinkId = DC.DrinkId
                    LEFT JOIN Category AS C ON DC.CategoryId = C.CategoryId
                    WHERE D.DrinkId = @DrinkId
                    GROUP BY D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL, B.BrandId, B.BrandName;";

                var parameters = new List<SqlParameter>
                {
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId },
                };

                var drinkQueryResult = this.dataBaseService.ExecuteSelectQuery(getDrinkQuery, parameters);

                if (drinkQueryResult == null || drinkQueryResult.Count == ZeroResults)
                {
                    return null;
                }

                var row = drinkQueryResult[FirstResult];
                int fetchedDrinkId = Convert.ToInt32(row["DrinkId"]);
                float alcoholContent = Convert.ToSingle(row["AlcoholContent"]);
                string drinkName = row["DrinkName"]?.ToString() ?? string.Empty;
                string drinkUrl = row["DrinkURL"]?.ToString() ?? string.Empty;
                int brandId = Convert.ToInt32(row["BrandId"]);
                string brandName = row["BrandName"]?.ToString() ?? string.Empty;

                var brand = new Brand(brandId, brandName);
                var categories = new List<Category>();

                if (row["CategoryIds"] != DBNull.Value && row["CategoryNames"] != DBNull.Value)
                {
                    var categoryIds = row["CategoryIds"].ToString().Split(',');
                    var categoryNames = row["CategoryNames"].ToString().Split(',');

                    for (int index = 0; index < categoryIds.Length; index++)
                    {
                        if (int.TryParse(categoryIds[index], out int categoryId))
                        {
                            categories.Add(new Category(categoryId, categoryNames[index]));
                        }
                    }
                }

                return new (fetchedDrinkId, drinkName, drinkUrl, categories, brand, alcoholContent);
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while getting drink by ID", exception);
            }
        }

        /// <summary>
        /// Retrieves drinks from the database based on various filters and search terms.
        /// </summary>
        /// <param name="searchTerm"> the search term. </param>
        /// <param name="brandNameFilters"> The brand filters. </param>
        /// <param name="categoryFilters"> The category filters. </param>
        /// <param name="minimumAlcohool"> minimum alcohol filter. </param>
        /// <param name="maximumAlcohool"> maximum alcohol filter. </param>
        /// <param name="orderBy"> How the result should be ordered. </param>
        /// <returns> The result list. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        public List<Drink> GetDrinks(
            string? searchTerm,
            List<string>? brandNameFilters,
            List<string>? categoryFilters,
            float? minimumAlcohool,
            float? maximumAlcohool,
            Dictionary<string, bool>? orderBy)
        {
            var drinks = new List<Drink>();

            try
            {
                string query = @"
                    SELECT D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL,
                           B.BrandId, B.BrandName,
                           STRING_AGG(C.CategoryId, ',') WITHIN GROUP (ORDER BY C.CategoryId) AS CategoryIds,
                           STRING_AGG(C.CategoryName, ',') WITHIN GROUP (ORDER BY C.CategoryId) AS CategoryNames
                    FROM Drink AS D
                    LEFT JOIN Brand AS B ON D.BrandId = B.BrandId
                    LEFT JOIN DrinkCategory AS DC ON D.DrinkId = DC.DrinkId
                    LEFT JOIN Category AS C ON DC.CategoryId = C.CategoryId";

                var conditions = new List<string>();
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var terms = searchTerm.Split(' ');
                    for (int index = 0; index < terms.Length; index++)
                    {
                        string paramName = "@SearchTerm" + index;
                        conditions.Add($"(LOWER(B.BrandName) LIKE {paramName} OR LOWER(C.CategoryName) LIKE {paramName} OR LOWER(D.DrinkName) LIKE {paramName})");
                        parameters.Add(new SqlParameter(paramName, SqlDbType.VarChar) { Value = "%" + terms[index].ToLower() + "%" });
                    }
                }

                if (brandNameFilters?.Count > ZeroResults)
                {
                    var brandParameters = brandNameFilters.Select((b, i) => "@Brand" + i).ToList();
                    conditions.Add("LOWER(LTRIM(RTRIM(B.BrandName))) IN (" + string.Join(", ", brandParameters) + ")");
                    for (int index = 0; index < brandNameFilters.Count; index++)
                    {
                        parameters.Add(new SqlParameter("@Brand" + index, SqlDbType.VarChar) { Value = brandNameFilters[index].Trim().ToLower() });
                    }
                }

                if (categoryFilters?.Count > ZeroResults)
                {
                    var categoryParameters = categoryFilters.Select((c, i) => "@Category" + i).ToList();
                    conditions.Add("LOWER(LTRIM(RTRIM(C.CategoryName))) IN (" + string.Join(", ", categoryParameters) + ")");
                    for (int index = 0; index < categoryFilters.Count; index++)
                    {
                        parameters.Add(new SqlParameter("@Category" + index, SqlDbType.VarChar) { Value = categoryFilters[index].Trim().ToLower() });
                    }
                }

                if (minimumAlcohool.HasValue)
                {
                    conditions.Add("D.AlcoholContent >= @MinAlcohol");
                    parameters.Add(new SqlParameter("@MinAlcohol", SqlDbType.Float) { Value = minimumAlcohool.Value });
                }

                if (maximumAlcohool.HasValue)
                {
                    conditions.Add("D.AlcoholContent <= @MaxAlcohol");
                    parameters.Add(new SqlParameter("@MaxAlcohol", SqlDbType.Float) { Value = maximumAlcohool.Value });
                }

                if (conditions.Count > ZeroResults)
                {
                    query += " WHERE " + string.Join(" AND ", conditions);
                }

                query += " GROUP BY D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL, B.BrandId, B.BrandName";
                query += " HAVING COUNT(DISTINCT B.BrandName) > 0";

                if (orderBy != null && orderBy.Count > ZeroResults)
                {
                    var orderClauses = orderBy.Select(order => order.Key + (order.Value ? " ASC" : " DESC"));
                    query += " ORDER BY " + string.Join(", ", orderClauses);
                }

                var drinkQueryResult = this.dataBaseService.ExecuteSelectQuery(query, parameters);

                foreach (var row in drinkQueryResult)
                {
                    int drinkId = Convert.ToInt32(row["DrinkId"]);
                    float alcoholContent = Convert.ToSingle(row["AlcoholContent"]);
                    string? drinkName = row["DrinkName"]?.ToString() ?? string.Empty;
                    string? imageUrl = row["DrinkURL"]?.ToString() ?? string.Empty;
                    int brandId = Convert.ToInt32(row["BrandId"]);
                    string? brandName = row["BrandName"]?.ToString() ?? string.Empty;
                    var brand = new Brand(brandId, brandName);

                    var categories = new List<Category>();
                    if (row["CategoryIds"] != DBNull.Value && row["CategoryNames"] != DBNull.Value)
                    {
                        var ids = row["CategoryIds"]?.ToString()?.Split(',');
                        var names = row["CategoryNames"]?.ToString()?.Split(',');

                        for (int index = 0; index < ids.Length; index++)
                        {
                            categories.Add(new Category(Convert.ToInt32(ids[index]), names[index]));
                        }
                    }

                    drinks.Add(new Drink(drinkId, drinkName, imageUrl, categories, brand, alcoholContent));
                }

                return drinks;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drinks.", exception);
            }
        }

        /// <summary>
        /// Adds a new drink to the database, creating the brand if it doesn't exist.
        /// </summary>
        /// <param name="drinkName">The name of the drink.</param>
        /// <param name="drinkUrl">The image URL of the drink.</param>
        /// <param name="categories">List of categories the drink belongs to.</param>
        /// <param name="brandName">The name of the drink's brand.</param>
        /// <param name="alcoholContent">The alcohol content of the drink.</param>
        public void AddDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent)
        {
            try
            {
                string brandIdQuery = @"SELECT BrandId 
                                        FROM Brand 
                                        WHERE BrandName = @BrandName";

                List<SqlParameter> brandNameParameter =
                [
                    new SqlParameter("@BrandName", SqlDbType.VarChar) { Value = brandName }
                ];
                var brandIdResult = this.dataBaseService.ExecuteSelectQuery(brandIdQuery, brandNameParameter);
                int brandId = brandIdResult.Count > ZeroResults ? Convert.ToInt32(brandIdResult[FirstResult]["BrandId"]) : NoResultFound;

                if (brandId == NoResultFound)
                {
                    string addBrandQuery = @"INSERT INTO Brand (BrandName) VALUES (@BrandName);
                                          SELECT SCOPE_IDENTITY() AS BrandId;";
                    List<SqlParameter> brandParameters =
                    [
                        new SqlParameter("@BrandName", SqlDbType.VarChar) { Value = brandName }
                    ];

                    var newBrandResult = this.dataBaseService.ExecuteSelectQuery(addBrandQuery, brandParameters);
                    brandId = Convert.ToInt32(newBrandResult[FirstResult]["BrandId"]);
                }

                string addDrinkQuery = @"INSERT INTO Drink (DrinkName, DrinkUrl, AlcoholContent, BrandId) 
                                VALUES (@DrinkName, @DrinkUrl, @AlcoholContent, @BrandId);
                                SELECT SCOPE_IDENTITY() AS DrinkId;";

                List<SqlParameter> drinkParameters =
                [
                    new SqlParameter("@DrinkName", SqlDbType.VarChar) { Value = drinkName },
                    new SqlParameter("@DrinkUrl", SqlDbType.VarChar) { Value = drinkUrl },
                    new SqlParameter("@AlcoholContent", SqlDbType.Float) { Value = alcoholContent },
                    new SqlParameter("@BrandId", SqlDbType.Int) { Value = brandId }
                ];

                var drinkIdResult = this.dataBaseService.ExecuteSelectQuery(addDrinkQuery, drinkParameters);
                int drinkId = Convert.ToInt32(drinkIdResult[FirstResult]["DrinkId"]);

                foreach (Category category in categories)
                {
                    string addCategoriesQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) 
                                               VALUES (@DrinkId, @CategoryId);";
                    List<SqlParameter> categoryParameters =
                    [
                        new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId },
                        new SqlParameter("@CategoryId", SqlDbType.Int) { Value = category.CategoryId }
                    ];

                    this.dataBaseService.ExecuteDataModificationQuery(addCategoriesQuery, categoryParameters);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred", exception);
            }
        }

        /// <summary>
        /// Deletes a drink by its ID.
        /// </summary>
        /// <param name="drinkId">The ID of the drink to delete.</param>
        public void DeleteDrink(int drinkId)
        {
            try
            {
                const string deleteQuery = "DELETE FROM Drink WHERE DrinkId = @DrinkId";

                var parameters = new List<SqlParameter>
                {
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId },
                };
                this.dataBaseService.ExecuteDataModificationQuery(deleteQuery, parameters);
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while deleting the drink.", exception);
            }
        }

        /// <summary>
        /// Updates an existing drink and its associated categories.
        /// </summary>
        /// <param name="drink">The updated drink object.</param>
        public void UpdateDrink(Drink drink)
        {
            try
            {
                const string brandIdQuery = @"SELECT BrandId FROM Brand WHERE BrandName = @BrandName";
                var brandParameters = new List<SqlParameter>
            {
                new ("@BrandName", SqlDbType.VarChar) { Value = drink.DrinkBrand.BrandName },
            };
                var brandResult = this.dataBaseService.ExecuteSelectQuery(brandIdQuery, brandParameters);
                int brandId = brandResult.Count > ZeroResults ? Convert.ToInt32(brandResult[FirstResult]["BrandId"]) : NoResultFound;
                if (brandId == NoResultFound)
                {
                    throw new Exception("Brand does not exist.");
                }

                const string updateDrinkQuery = @"
                UPDATE Drink SET
                AlcoholContent = @AlcoholContent,
                BrandId = @BrandId,
                DrinkName = @DrinkName,
                DrinkUrl = @DrinkUrl
                WHERE DrinkId = @DrinkId;";

                var updateDrinkQuerryParameters = new List<SqlParameter>
            {
                new ("@DrinkName", SqlDbType.VarChar) { Value = drink.DrinkName },
                new ("@DrinkUrl", SqlDbType.VarChar) { Value = drink.DrinkImageUrl },
                new ("@AlcoholContent", SqlDbType.Float) { Value = drink.AlcoholContent },
                new ("@BrandId", SqlDbType.Int) { Value = brandId },
                new ("@DrinkId", SqlDbType.Int) { Value = drink.DrinkId },
            };
                this.dataBaseService.ExecuteDataModificationQuery(updateDrinkQuery, updateDrinkQuerryParameters);

                const string getExistingCategoriesForADrinkQuery = @"SELECT CategoryId FROM DrinkCategory WHERE DrinkId = @DrinkId";
                var getAllCategoriesForADrinkQuerryParameters = new List<SqlParameter>
            {
                new ("@DrinkId", SqlDbType.Int) { Value = drink.DrinkId },
            };
                var existingCategoriesResult = this.dataBaseService.ExecuteSelectQuery(getExistingCategoriesForADrinkQuery, getAllCategoriesForADrinkQuerryParameters);
                var existingCategoryIds = new HashSet<int>(existingCategoriesResult.Select(row => Convert.ToInt32(row["CategoryId"])));
                var newCategoryIds = new HashSet<int>(drink.CategoryList.Select(c => c.CategoryId));

                var categoriesToInsert = newCategoryIds.Except(existingCategoryIds);
                var categoriesToDelete = existingCategoryIds.Except(newCategoryIds);

                foreach (var categoryId in categoriesToInsert)
                {
                    const string insertCategoryQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (@DrinkId, @CategoryId);";
                    var insertCategoryQueryParameters = new List<SqlParameter>
                {
                new ("@DrinkId", SqlDbType.Int) { Value = drink.DrinkId },
                new ("@CategoryId", SqlDbType.Int) { Value = categoryId },
                };
                    this.dataBaseService.ExecuteDataModificationQuery(insertCategoryQuery, insertCategoryQueryParameters);
                }

                foreach (var categoryId in categoriesToDelete)
                {
                    const string deleteCategoryQuery = @"DELETE FROM DrinkCategory WHERE DrinkId = @DrinkId AND CategoryId = @CategoryId";
                    var deleteCatergoryForADrinkQueryParameters = new List<SqlParameter>
                {
                    new ("@DrinkId", SqlDbType.Int) { Value = drink.DrinkId },
                    new ("@CategoryId", SqlDbType.Int) { Value = categoryId },
                };
                    this.dataBaseService.ExecuteDataModificationQuery(deleteCategoryQuery, deleteCatergoryForADrinkQueryParameters);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while updating the drink.", exception);
            }
        }

        /// <summary>
        /// Retrieves all drink categories from the database.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        public List<Category> GetDrinkCategories()
        {
            List<Category> categories = new List<Category>();

            try
            {
                string getCategoriesQuery = "SELECT * FROM Category ORDER BY CategoryId;";
                var selectResult = this.dataBaseService.ExecuteSelectQuery(getCategoriesQuery);

                foreach (var row in selectResult)
                {
                    int id = Convert.ToInt32(row["CategoryId"]);
                    string name = row["CategoryName"].ToString();
                    categories.Add(new Category(id, name));
                }

                return categories;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drink categories.", exception);
            }
        }

        /// <summary>
        /// Retrieves all drink brands from the database.
        /// </summary>
        /// <returns>A list of all brands.</returns>
        public List<Brand> GetDrinkBrands()
        {
            try
            {
                string getBrandsQuery = "SELECT * FROM Brand ORDER BY BrandId;";
                var selectResult = this.dataBaseService.ExecuteSelectQuery(getBrandsQuery);
                List<Brand> brands = new List<Brand>();
                foreach (var row in selectResult)
                {
                    int id = Convert.ToInt32(row["BrandId"]);
                    string name = row["BrandName"].ToString();
                    brands.Add(new Brand(id, name));
                }

                return brands;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drink brands.", exception);
            }
        }

        /// <summary>
        /// Retrieves the personal drink list for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="numberOfDrinks">Number of drinks to return.</param>
        /// <returns>List of drinks for the user.</returns>
        public List<Drink> GetPersonalDrinkList(int userId, int numberOfDrinks = 1)
        {
            const string query = @"
                SELECT d.DrinkId, d.AlcoholContent, d.DrinkName, d.DrinkURL,
                       b.BrandId, b.BrandName,
                       STRING_AGG(c.CategoryId, ',') WITHIN GROUP (ORDER BY c.CategoryId) AS CategoryIds,
                       STRING_AGG(c.CategoryName, ',') WITHIN GROUP (ORDER BY c.CategoryId) AS CategoryNames
                FROM UserDrink ud
                JOIN Drink d ON ud.DrinkId = d.DrinkId
                JOIN Brand b ON d.BrandId = b.BrandId
                LEFT JOIN DrinkCategory dc ON d.DrinkId = dc.DrinkId
                LEFT JOIN Category c ON dc.CategoryId = c.CategoryId
                WHERE ud.UserId = @UserId
                GROUP BY d.DrinkId, d.AlcoholContent, d.DrinkName, d.DrinkURL, b.BrandId, b.BrandName
                ORDER BY d.DrinkId;";

            var parameters = new List<SqlParameter>
            {
                new ("@UserId", SqlDbType.Int) { Value = userId },
                new ("@NumberOfDrinks", SqlDbType.Int) { Value = numberOfDrinks },
            };

            try
            {
                var selectResult = this.dataBaseService.ExecuteSelectQuery(query, parameters);
                var drinks = new List<Drink>();

                foreach (var row in selectResult)
                {
                    int drinkId = Convert.ToInt32(row["DrinkId"]);
                    float alcoholContent = Convert.ToSingle(row["AlcoholContent"]);
                    string drinkName = row["DrinkName"].ToString();
                    string imageUrl = row["DrinkURL"].ToString();

                    int brandId = Convert.ToInt32(row["BrandId"]);
                    string brandName = row["BrandName"].ToString();
                    var brand = new Brand(brandId, brandName);

                    var categories = new List<Category>();
                    if (row["CategoryIds"] != DBNull.Value && row["CategoryNames"] != DBNull.Value)
                    {
                        var ids = row["CategoryIds"].ToString().Split(',');
                        var names = row["CategoryNames"].ToString().Split(',');
                        for (int index = 0; index < ids.Length; index++)
                        {
                            categories.Add(new Category(Convert.ToInt32(ids[index]), names[index]));
                        }
                    }

                    drinks.Add(new Drink(drinkId, drinkName, imageUrl, categories, brand, alcoholContent));
                }

                return drinks;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving user's personal drink list.", exception);
            }
        }

        /// <summary>
        /// Checks if a drink is in the user's personal drink list.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="drinkId">The drink ID.</param>
        /// <returns>True if the drink is in the list, false otherwise.</returns>
        public bool IsDrinkInPersonalList(int userId, int drinkId)
        {
            try
            {
                const string query = "SELECT COUNT(*) AS DrinkCount FROM UserDrink WHERE UserId = @UserId AND DrinkId = @DrinkId;";

                var parameters = new List<SqlParameter>
                {
                    new ("@UserId", SqlDbType.Int) { Value = userId },
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId },
                };

                var result = this.dataBaseService.ExecuteSelectQuery(query, parameters);

                if (result != null && result.Count > ZeroResults)
                {
                    int count = Convert.ToInt32(result[FirstResult]["DrinkCount"]);
                    return count > ZeroResults;
                }

                return false;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while checking personal drink list.", exception);
            }
        }

        /// <summary>
        /// Adds a drink to the user's personal drink list.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="drinkId">The ID of the drink to add.</param>
        /// <returns>True if the drink was successfully added; otherwise, false.</returns>
        public bool AddToPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                const string insertQuery = "INSERT INTO UserDrink (UserId, DrinkId) VALUES (@UserId, @DrinkId);";

                var parameters = new List<SqlParameter>
                {
                    new ("@UserId", SqlDbType.Int) { Value = userId },
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId },
                };

                int rowsAffected = this.dataBaseService.ExecuteDataModificationQuery(insertQuery, parameters);

                return rowsAffected > ZeroResults;
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to add drink to personal list.", exception);
            }
        }

        /// <summary>
        /// Deletes a drink from the user's personal drink list.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="drinkId">The ID of the drink to remove.</param>
        /// <returns>True if the drink was successfully removed; otherwise, false.</returns>
        public bool DeleteFromPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                const string deleteQuery = "DELETE FROM UserDrink WHERE UserId = @UserId AND DrinkId = @DrinkId;";

                var parameters = new List<SqlParameter>
                {
                    new ("@UserId", SqlDbType.Int) { Value = userId },
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId },
                };

                int rowsAffected = this.dataBaseService.ExecuteDataModificationQuery(deleteQuery, parameters);

                return rowsAffected > ZeroResults;
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to delete drink from personal list.", exception);
            }
        }

        /// <summary>
        /// Casts or updates a user's vote for Drink of the Day.
        /// </summary>
        /// <param name="userId">The ID of the user voting.</param>
        /// <param name="drinkId">The ID of the drink to vote for.</param>
        public void VoteDrinkOfTheDay(int userId, int drinkId)
        {
            DateTime voteTime = DateTime.UtcNow;

            try
            {
                const string checkQuery = @"
                    SELECT COUNT(*) AS VoteCount FROM Vote
                    WHERE UserId = @UserId
                    AND CONVERT(date, VoteTime) = CONVERT(date, @VoteTime);";

                var checkParams = new List<SqlParameter>
                {
                    new ("@UserId", SqlDbType.Int) { Value = userId },
                    new ("@VoteTime", SqlDbType.DateTime) { Value = voteTime },
                };

                var result = this.dataBaseService.ExecuteSelectQuery(checkQuery, checkParams);
                int count = Convert.ToInt32(result[FirstResult]["VoteCount"]);

                if (count > ZeroResults)
                {
                    const string updateQuery = @"
                        UPDATE Vote SET DrinkId = @DrinkId, VoteTime = @VoteTime
                        WHERE UserId = @UserId
                        AND CONVERT(date, VoteTime) = CONVERT(date, @VoteTime);";

                    var updateParams = new List<SqlParameter>
                    {
                        new ("@DrinkId", SqlDbType.Int) { Value = drinkId },
                        new ("@UserId", SqlDbType.Int) { Value = userId },
                        new ("@VoteTime", SqlDbType.DateTime) { Value = voteTime },
                    };

                    this.dataBaseService.ExecuteDataModificationQuery(updateQuery, updateParams);
                }
                else
                {
                    const string insertQuery = @"
                        INSERT INTO Vote (UserId, DrinkId, VoteTime)
                        VALUES (@UserId, @DrinkId, @VoteTime);";

                    var insertParams = new List<SqlParameter>
                    {
                        new ("@UserId", SqlDbType.Int) { Value = userId },
                        new ("@DrinkId", SqlDbType.Int) { Value = drinkId },
                        new ("@VoteTime", SqlDbType.DateTime) { Value = voteTime },
                    };

                    this.dataBaseService.ExecuteDataModificationQuery(insertQuery, insertParams);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to vote for drink of the day.", exception);
            }
        }

        /// <summary>
        /// Retrieves the Drink of the Day. Resets it if the current date has changed.
        /// </summary>
        /// <returns>The Drink of the Day.</returns>
        public Drink GetDrinkOfTheDay()
        {
            try
            {
                string getDrinkOfTheDayQuery = "SELECT * FROM DrinkOfTheDay;";
                var drinkQueryResult = this.dataBaseService.ExecuteSelectQuery(getDrinkOfTheDayQuery);

                if (drinkQueryResult.Count == ZeroResults)
                {
                    this.SetDrinkOfTheDay();
                    drinkQueryResult = this.dataBaseService.ExecuteSelectQuery(getDrinkOfTheDayQuery);
                }

                int drinkId = Convert.ToInt32(drinkQueryResult[FirstResult]["DrinkId"]);
                DateTime drinkTime = Convert.ToDateTime(drinkQueryResult[FirstResult]["DrinkTime"]);

                if (drinkTime.Date != DateTime.UtcNow.Date)
                {
                    this.ResetDrinkOfTheDay();
                    drinkQueryResult = this.dataBaseService.ExecuteSelectQuery(getDrinkOfTheDayQuery);
                    drinkId = Convert.ToInt32(drinkQueryResult[FirstResult]["DrinkId"]);
                }

                const string getDrinkQuery = "SELECT D.BrandId, D.DrinkName, D.DrinkURL, D.AlcoholContent FROM Drink AS D WHERE D.DrinkId = @DrinkId;";
                var drinkParams = new List<SqlParameter>
                {
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId },
                };
                drinkQueryResult = this.dataBaseService.ExecuteSelectQuery(getDrinkQuery, drinkParams);

                if (drinkQueryResult.Count == ZeroResults)
                {
                    throw new Exception($"No drink of the day with id {drinkId} found.");
                }

                int brandId = Convert.ToInt32(drinkQueryResult[FirstResult]["BrandId"]);
                string drinkName = drinkQueryResult[FirstResult]["DrinkName"]?.ToString() ?? string.Empty;
                string drinkImageUrl = drinkQueryResult[FirstResult]["DrinkURL"]?.ToString() ?? string.Empty;
                float alcoholContent = (float)Convert.ToDouble(drinkQueryResult[FirstResult]["AlcoholContent"]);

                const string getCategoriesQuery = "SELECT C.CategoryId, C.CategoryName FROM Category AS C JOIN DrinkCategory AS DC ON DC.CategoryId = C.CategoryId WHERE DC.DrinkId = @DrinkId;";
                var categoryResult = this.dataBaseService.ExecuteSelectQuery(getCategoriesQuery, drinkParams);

                var categories = new List<Category>();
                foreach (var row in categoryResult)
                {
                    int categoryId = Convert.ToInt32(row["CategoryId"]);
                    string categoryName = row["CategoryName"]?.ToString() ?? string.Empty;
                    categories.Add(new Category(categoryId, categoryName));
                }

                const string getBrandQuery = "SELECT BrandName FROM Brand WHERE BrandId = @BrandId;";
                var brandParams = new List<SqlParameter>
                {
                    new ("@BrandId", SqlDbType.Int) { Value = brandId },
                };
                var brandQueryResult = this.dataBaseService.ExecuteSelectQuery(getBrandQuery, brandParams);

                string brandName = brandQueryResult[FirstResult]["BrandName"]?.ToString() ?? string.Empty;
                var brand = new Brand(brandId, brandName);

                return new Drink(drinkId, drinkName, drinkImageUrl, categories, brand, alcoholContent);
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to retrieve drink of the day.", exception);
            }
        }

        /// <summary>
        /// Retrieves the drink ID with the highest number of votes today.
        /// </summary>
        /// <returns>The drink ID with the most votes.</returns>
        public int GetCurrentTopVotedDrink()
        {
            const string topVoteCountQuery = @"
                SELECT TOP 1 DrinkId, COUNT(*) AS VoteCount
                FROM Vote
                WHERE CONVERT(date, VoteTime) >= CONVERT(date, @VoteTime)
                GROUP BY DrinkId
                ORDER BY COUNT(*) DESC;";

            var voteDayParameter = new List<SqlParameter>
            {
                new ("@VoteTime", SqlDbType.DateTime) { Value = DateTime.UtcNow.Date.AddDays(-1) },
            };

            var topVoteCountResult = this.dataBaseService.ExecuteSelectQuery(topVoteCountQuery, voteDayParameter);

            if (topVoteCountResult.Count == ZeroResults)
            {
                return this.GetRandomDrinkId();
            }

            return Convert.ToInt32(topVoteCountResult[FirstResult]["DrinkId"]);
        }

        /// <summary>
        /// Gets a random drink ID from the database.
        /// </summary>
        /// <returns>A random drink ID or -1 if no drinks are available.</returns>
        public int GetRandomDrinkId()
        {
            try
            {
                const string getRandomDrinkIdQuery = "SELECT TOP 1 DrinkId FROM Drink ORDER BY NEWID();";
                var selectResult = this.dataBaseService.ExecuteSelectQuery(getRandomDrinkIdQuery);
                if (selectResult.Count > ZeroResults)
                {
                    return Convert.ToInt32(selectResult[FirstResult]["DrinkId"]);
                }

                return NoResultFound;
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to retrieve random drink ID.", exception);
            }
        }

        /// <summary>
        /// Sets the Drink of the Day to the top-voted drink.
        /// </summary>
        private void SetDrinkOfTheDay()
        {
            try
            {
                int topVotedDrinkId = this.GetCurrentTopVotedDrink();

                const string insertQuery = @"
                        INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime)
                        VALUES (@DrinkId, GETDATE());";

                var parameters = new List<SqlParameter>
                    {
                        new ("@DrinkId", SqlDbType.Int) { Value = topVotedDrinkId },
                    };

                this.dataBaseService.ExecuteDataModificationQuery(insertQuery, parameters);
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to set drink of the day.", exception);
            }
        }

        /// <summary>
        /// Resets the Drink of the Day to the new top-voted drink for today.
        /// </summary>
        private void ResetDrinkOfTheDay()
        {
            try
            {
                const string deleteQuery = "DELETE FROM DrinkOfTheDay;";
                this.dataBaseService.ExecuteDataModificationQuery(deleteQuery, null);
                this.SetDrinkOfTheDay();
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to reset drink of the day.", exception);
            }
        }
    }
}