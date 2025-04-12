using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using WinUIApp.Services;
using Microsoft.IdentityModel.Tokens;

namespace WinUIApp.Models
{
    /// <summary>
    /// Handles all database operations related to drinks.
    /// </summary>
    class DrinkModel
    {
        private const float MaxAlcoholContent = 100f;

        readonly DatabaseService dataBaseService;
        public DrinkModel()
        {
            dataBaseService = DatabaseService.Instance;
        }
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
                    new ("@DrinkId", SqlDbType.Int){ Value = drinkId }
                };

                var drinkQueryResult = dataBaseService.ExecuteSelectQuery(getDrinkQuery, parameters);
                
                if (drinkQueryResult == null || drinkQueryResult.Count == 0)
                {
                    return null;
                }

                var row = drinkQueryResult[0];
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
                    
                    for (int i = 0; i < categoryIds.Length; i++)
                    {
                        if (int.TryParse(categoryIds[i], out int categoryId))
                        {
                            categories.Add(new Category(categoryId, categoryNames[i]));
                        }
                    }
                }

                return new (fetchedDrinkId, drinkName, drinkUrl, categories, brand, alcoholContent);
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred while getting drink by ID", ex);
            }
        }

        /// <summary>
        /// Retrieves drinks matching the specified search and filter criteria.
        /// </summary>
        public List<Drink> GetDrinks(
            string? searchTerm,
            List<string>? brandNameFilters,
            List<string>? categoryFilters,
            float? minAlcohol,
            float? maxAlcohol,
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
                    for (int i = 0; i < terms.Length; i++)
                    {
                        string paramName = "@SearchTerm" + i;
                        conditions.Add($"(LOWER(B.BrandName) LIKE {paramName} OR LOWER(C.CategoryName) LIKE {paramName} OR LOWER(D.DrinkName) LIKE {paramName})");
                        parameters.Add(new SqlParameter(paramName, SqlDbType.VarChar) { Value = "%" + terms[i].ToLower() + "%" });
                    }
                }

                if (brandNameFilters?.Count > 0)
                {
                    var brandParams = brandNameFilters.Select((b, i) => "@Brand" + i).ToList();
                    conditions.Add("LOWER(LTRIM(RTRIM(B.BrandName))) IN (" + string.Join(", ", brandParams) + ")");
                    for (int i = 0; i < brandNameFilters.Count; i++)
                    {
                        parameters.Add(new SqlParameter("@Brand" + i, SqlDbType.VarChar) { Value = brandNameFilters[i].Trim().ToLower() });
                    }
                }

                if (categoryFilters?.Count > 0)
                {
                    var categoryParams = categoryFilters.Select((c, i) => "@Category" + i).ToList();
                    conditions.Add("LOWER(LTRIM(RTRIM(C.CategoryName))) IN (" + string.Join(", ", categoryParams) + ")");
                    for (int i = 0; i < categoryFilters.Count; i++)
                    {
                        parameters.Add(new SqlParameter("@Category" + i, SqlDbType.VarChar) { Value = categoryFilters[i].Trim().ToLower() });
                    }
                }

                if (minAlcohol.HasValue)
                {
                    conditions.Add("D.AlcoholContent >= @MinAlcohol");
                    parameters.Add(new SqlParameter("@MinAlcohol", SqlDbType.Float) { Value = minAlcohol.Value });
                }

                if (maxAlcohol.HasValue)
                {
                    conditions.Add("D.AlcoholContent <= @MaxAlcohol");
                    parameters.Add(new SqlParameter("@MaxAlcohol", SqlDbType.Float) { Value = maxAlcohol.Value });
                }

                if (conditions.Count > 0)
                {
                    query += " WHERE " + string.Join(" AND ", conditions);
                }

                query += " GROUP BY D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL, B.BrandId, B.BrandName";
                query += " HAVING COUNT(DISTINCT B.BrandName) > 0";

                if (orderBy != null && orderBy.Count > 0)
                {
                    var orderClauses = orderBy.Select(o => o.Key + (o.Value ? " ASC" : " DESC"));
                    query += " ORDER BY " + string.Join(", ", orderClauses);
                }


                var drinkQueryResult = dataBaseService.ExecuteSelectQuery(query, parameters);

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
                            
                        for (int i = 0; i < ids.Length; i++)
                        {
                            categories.Add(new Category(Convert.ToInt32(ids[i]), names[i]));
                        }
                    }

                    drinks.Add(new Drink(drinkId, drinkName, imageUrl, categories, brand, alcoholContent));
                }

                return drinks;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred while retrieving drinks.", ex);
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
                var brandIdResult = dataBaseService.ExecuteSelectQuery(brandIdQuery, brandNameParameter);
                int brandId = brandIdResult.Count > 0 ? Convert.ToInt32(brandIdResult[0]["BrandId"]) : -1;

                if (brandId == -1)
                {
                    string addBrandQuery = @"INSERT INTO Brand (BrandName) VALUES (@BrandName);
                                          SELECT SCOPE_IDENTITY() AS BrandId;";
                    List<SqlParameter> brandParameters =
                    [
                        new SqlParameter("@BrandName", SqlDbType.VarChar) { Value = brandName }
                    ];

                    var newBrandResult = dataBaseService.ExecuteSelectQuery(addBrandQuery, brandParameters);
                    brandId = Convert.ToInt32(newBrandResult[0]["BrandId"]);
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

                var drinkIdResult = dataBaseService.ExecuteSelectQuery(addDrinkQuery, drinkParameters);
                int drinkId = Convert.ToInt32(drinkIdResult[0]["DrinkId"]);

                foreach (Category category in categories)
                {
                    string addCategoriesQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) 
                                               VALUES (@DrinkId, @CategoryId);";
                    List<SqlParameter> categoryParameters =
                    [
                        new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId },
                        new SqlParameter("@CategoryId", SqlDbType.Int) { Value = category.CategoryId }
                    ];

                    dataBaseService.ExecuteDataModificationQuery(addCategoriesQuery, categoryParameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
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
                    new("@DrinkId", SqlDbType.Int) { Value = drinkId }
                };
                dataBaseService.ExecuteDataModificationQuery(deleteQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred while deleting the drink.", ex);
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
            // Fetch brand ID
            const string brandIdQuery = @"SELECT BrandId FROM Brand WHERE BrandName = @BrandName";
            var brandParams = new List<SqlParameter>
            {
                new ("@BrandName", SqlDbType.VarChar) { Value = drink.DrinkBrand.BrandName }
            };
            var brandResult = dataBaseService.ExecuteSelectQuery(brandIdQuery, brandParams);
            int brandId = brandResult.Count > 0 ? Convert.ToInt32(brandResult[0]["BrandId"]) : -1;
            if (brandId == -1)
            {
                throw new Exception("Brand does not exist.");
            }

            // Update drink details
            const string updateDrinkQuery = @"
                UPDATE Drink SET
                AlcoholContent = @AlcoholContent,
                BrandId = @BrandId,
                DrinkName = @DrinkName,
                DrinkUrl = @DrinkUrl
                WHERE DrinkId = @DrinkId;";

            var drinkParams = new List<SqlParameter>
            {
                new ("@DrinkName", SqlDbType.VarChar) { Value = drink.DrinkName },
                new ("@DrinkUrl", SqlDbType.VarChar) { Value = drink.DrinkImageUrl },
                new ("@AlcoholContent", SqlDbType.Float) { Value = drink.AlcoholContent },
                new ("@BrandId", SqlDbType.Int) { Value = brandId },
                new ("@DrinkId", SqlDbType.Int) { Value = drink.DrinkId }
            };
            dataBaseService.ExecuteDataModificationQuery(updateDrinkQuery, drinkParams);

            // Fetch existing categories
            const string getExistingCategoriesQuery = @"SELECT CategoryId FROM DrinkCategory WHERE DrinkId = @DrinkId";
            var categoryParams = new List<SqlParameter>
            {
                new ("@DrinkId", SqlDbType.Int) { Value = drink.DrinkId }
            };
            var existingCategoriesResult = dataBaseService.ExecuteSelectQuery(getExistingCategoriesQuery, categoryParams);
            var existingCategoryIds = new HashSet<int>(existingCategoriesResult.Select(row => Convert.ToInt32(row["CategoryId"])));
            var newCategoryIds = new HashSet<int>(drink.CategoryList.Select(c => c.CategoryId));

            // Determine categories to insert and delete
            var categoriesToInsert = newCategoryIds.Except(existingCategoryIds);
            var categoriesToDelete = existingCategoryIds.Except(newCategoryIds);

            // Insert new categories
            foreach (var categoryId in categoriesToInsert)
            {
                const string insertCategoryQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (@DrinkId, @CategoryId);";
                var insertParams = new List<SqlParameter>
                {
                new ("@DrinkId", SqlDbType.Int) { Value = drink.DrinkId },
                new ("@CategoryId", SqlDbType.Int) { Value = categoryId }
                };
                dataBaseService.ExecuteDataModificationQuery(insertCategoryQuery, insertParams);
            }

            // Delete removed categories
            foreach (var categoryId in categoriesToDelete)
            {
                const string deleteCategoryQuery = @"DELETE FROM DrinkCategory WHERE DrinkId = @DrinkId AND CategoryId = @CategoryId";
                var deleteParams = new List<SqlParameter>
                {
                    new ("@DrinkId", SqlDbType.Int) { Value = drink.DrinkId },
                    new ("@CategoryId", SqlDbType.Int) { Value = categoryId }
                };
                dataBaseService.ExecuteDataModificationQuery(deleteCategoryQuery, deleteParams);
            }
            }
            catch (Exception ex)
            {
            throw new Exception("Database error occurred while updating the drink.", ex);
            }
        }

        /// <summary>
        /// Retrieves all drink categories from the database.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        public List<Category> GetDrinkCategories()
        {
            List<Category> categories = [];

            try
            {
                string getCategoriesQuery = "SELECT * FROM Category ORDER BY CategoryId;";
                var selectResult = dataBaseService.ExecuteSelectQuery(getCategoriesQuery);

                foreach (var row in selectResult)
                {
                    int id = Convert.ToInt32(row["CategoryId"]);
                    string name = row["CategoryName"].ToString();
                    categories.Add(new Category(id, name));
                }

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred while retrieving drink categories.", ex);
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
                var selectResult = dataBaseService.ExecuteSelectQuery(getBrandsQuery);
                List<Brand> brands = [];
                foreach (var row in selectResult)
                {
                    int id = Convert.ToInt32(row["BrandId"]);
                    string name = row["BrandName"].ToString();
                    brands.Add(new Brand(id, name));
                }

                return brands;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred while retrieving drink brands.", ex);
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
                new ("@NumberOfDrinks", SqlDbType.Int) { Value = numberOfDrinks }
            };

            try
            {
                var selectResult = dataBaseService.ExecuteSelectQuery(query, parameters);
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
                        for (int i = 0; i < ids.Length; i++)
                        {
                            categories.Add(new Category(Convert.ToInt32(ids[i]), names[i]));
                        }
                    }

                    drinks.Add(new Drink(drinkId, drinkName, imageUrl, categories, brand, alcoholContent));
                }

                return drinks;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred while retrieving user's personal drink list.", ex);
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
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId }
                };

                var result = dataBaseService.ExecuteSelectQuery(query, parameters);

                if (result != null && result.Count > 0)
                {
                    int count = Convert.ToInt32(result[0]["DrinkCount"]);
                    return count > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred while checking personal drink list.", ex);
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
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId }
                };

                int rowsAffected = dataBaseService.ExecuteDataModificationQuery(insertQuery, parameters);


                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add drink to personal list.", ex);
            }
        }

        /// <summary>
        /// Deletes a drink from the user's personal drink list.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="drinkId">The ID of the drink to remove.</param>
        /// <returns>True if the drink was successfully removed; otherwise, false.</returns>
        public  bool DeleteFromPersonalDrinkList(int userId, int drinkId)
        {

            try
            {
                const string deleteQuery = "DELETE FROM UserDrink WHERE UserId = @UserId AND DrinkId = @DrinkId;";

                var parameters = new List<SqlParameter>
                {
                    new ("@UserId", SqlDbType.Int) { Value = userId },
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId }
                };

                int rowsAffected = dataBaseService.ExecuteDataModificationQuery(deleteQuery, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete drink from personal list.", ex);
            }
        }


        /// <summary>
        /// Casts or updates a user's vote for Drink of the Day.
        /// </summary>
        /// <param name="drinkId">The ID of the drink to vote for.</param>
        /// <param name="userId">The ID of the user voting.</param>
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
                    new ("@VoteTime", SqlDbType.DateTime) { Value = voteTime }
                };

                var result = dataBaseService.ExecuteSelectQuery(checkQuery, checkParams);
                int count = Convert.ToInt32(result[0]["VoteCount"]);

                if (count > 0)
                {
                    const string updateQuery = @"
                        UPDATE Vote SET DrinkId = @DrinkId, VoteTime = @VoteTime
                        WHERE UserId = @UserId
                        AND CONVERT(date, VoteTime) = CONVERT(date, @VoteTime);";

                    var updateParams = new List<SqlParameter>
                    {
                        new ("@DrinkId", SqlDbType.Int) { Value = drinkId },
                        new ("@UserId", SqlDbType.Int) { Value = userId },
                        new ("@VoteTime", SqlDbType.DateTime) { Value = voteTime }
                    };

                    dataBaseService.ExecuteDataModificationQuery(updateQuery, updateParams);
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
                        new ("@VoteTime", SqlDbType.DateTime) { Value = voteTime }
                    };

                    dataBaseService.ExecuteDataModificationQuery(insertQuery, insertParams);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to vote for drink of the day.", ex);
            }
        }

        /// <summary>
        /// Retrieves the Drink of the Day. Resets it if the current date has changed.
        /// </summary>
        /// <returns>The Drink of the Day.</returns>
        public Drink GetDrinkOfTheDay()
        {
            var dataBaseService = DatabaseService.Instance;

            try
            {
                string getDrinkOfTheDayQuery = "SELECT * FROM DrinkOfTheDay;";
                var drinkQueryResult = dataBaseService.ExecuteSelectQuery(getDrinkOfTheDayQuery);

                if (drinkQueryResult.Count == 0)
                {
                    SetDrinkOfTheDay();
                    drinkQueryResult = dataBaseService.ExecuteSelectQuery(getDrinkOfTheDayQuery);
                }

                int drinkId = Convert.ToInt32(drinkQueryResult[0]["DrinkId"]);
                DateTime drinkTime = Convert.ToDateTime(drinkQueryResult[0]["DrinkTime"]);

                if (drinkTime.Date != DateTime.UtcNow.Date)
                {
                    ResetDrinkOfTheDay();
                    drinkQueryResult = dataBaseService.ExecuteSelectQuery(getDrinkOfTheDayQuery);
                    drinkId = Convert.ToInt32(drinkQueryResult[0]["DrinkId"]);
                }

                const string getDrinkQuery = "SELECT D.BrandId, D.DrinkName, D.DrinkURL, D.AlcoholContent FROM Drink AS D WHERE D.DrinkId = @DrinkId;";
                var drinkParams = new List<SqlParameter>
                {
                    new ("@DrinkId", SqlDbType.Int) { Value = drinkId }
                };
                drinkQueryResult = dataBaseService.ExecuteSelectQuery(getDrinkQuery, drinkParams);

                if (drinkQueryResult.Count == 0)
                {
                    throw new Exception($"No drink of the day with id {drinkId} found.");
                }

                int brandId = Convert.ToInt32(drinkQueryResult[0]["BrandId"]);
                string drinkName = drinkQueryResult[0]["DrinkName"]?.ToString() ?? string.Empty;
                string drinkImageUrl = drinkQueryResult[0]["DrinkURL"]?.ToString() ?? string.Empty;
                float alcoholContent = (float)Convert.ToDouble(drinkQueryResult[0]["AlcoholContent"]);

                const string getCategoriesQuery = "SELECT C.CategoryId, C.CategoryName FROM Category AS C JOIN DrinkCategory AS DC ON DC.CategoryId = C.CategoryId WHERE DC.DrinkId = @DrinkId;";
                var categoryResult = dataBaseService.ExecuteSelectQuery(getCategoriesQuery, drinkParams);

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
                    new ("@BrandId", SqlDbType.Int) { Value = brandId }
                };
                var brandQueryResult = dataBaseService.ExecuteSelectQuery(getBrandQuery, brandParams);

                string brandName = brandQueryResult[0]["BrandName"]?.ToString() ?? string.Empty;
                var brand = new Brand(brandId, brandName);

                return new Drink(drinkId, drinkName, drinkImageUrl, categories, brand, alcoholContent);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve drink of the day." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Retrieves the drink ID with the highest number of votes today.
        /// </summary>
        /// <returns>The drink ID with the most votes.</returns>
        public int GetCurrentTopVotedDrink()
        {
            var dataBaseService = DatabaseService.Instance;

            const string topVoteCountQuery = @"
                SELECT TOP 1 DrinkId, COUNT(*) AS VoteCount
                FROM Vote
                WHERE CONVERT(date, VoteTime) >= CONVERT(date, @VoteTime)
                GROUP BY DrinkId
                ORDER BY COUNT(*) DESC;";

            List<SqlParameter> voteDayParameter =
            [
                new SqlParameter("@VoteTime", SqlDbType.DateTime) { Value = DateTime.UtcNow.Date.AddDays(-1) }
            ];

            var topVoteCountResult = dataBaseService.ExecuteSelectQuery(topVoteCountQuery, voteDayParameter);

            if (topVoteCountResult.Count == 0)
            {
                return GetRandomDrinkId();
            }

            return Convert.ToInt32(topVoteCountResult[0]["DrinkId"]);
        }

        public int GetRandomDrinkId()
        {
            try
            {
                // SQL Server uses NEWID() for random ordering
                string getRandomDrinkIdQuery = "SELECT TOP 1 DrinkId FROM Drink ORDER BY NEWID();";
                var selectResult = dataBaseService.ExecuteSelectQuery(getRandomDrinkIdQuery);
                if (selectResult.Count > 0)
                {
                    return Convert.ToInt32(selectResult[0]["DrinkId"]);
                }

                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve top-voted drink.", ex);
            }
        }

        /// <summary>
        /// Sets the Drink of the Day to the top-voted drink.
        /// </summary>
        public void SetDrinkOfTheDay()
        {
            try
            {
                //Temporarily removed WHERE CONVERT(date, VoteTime) = CONVERT(date, GETDATE()) from query due to
                // the fact that it runs on flawed logic and can cause the application to not run.
                string insertQuery = @"
                    INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime)
                    SELECT TOP 1 DrinkId, GETDATE()
                    FROM Vote
                    GROUP BY DrinkId
                    ORDER BY COUNT(*) DESC;";

                dataBaseService.ExecuteDataModificationQuery(insertQuery, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to set drink of the day.", ex);
            }
        }
        /// <summary>
        /// Resets the Drink of the Day to the new top-voted drink for today.
        /// </summary>
        public void ResetDrinkOfTheDay()
        {
            var dataBaseService = DatabaseService.Instance;

            try
            {
                const string deleteQuery = "DELETE FROM DrinkOfTheDay;";
                dataBaseService.ExecuteDataModificationQuery(deleteQuery, null);
                SetDrinkOfTheDay();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to reset drink of the day.", ex);
            }
        }
    }
}
