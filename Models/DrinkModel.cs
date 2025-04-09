using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using WinUIApp.Services;

namespace WinUIApp.Models
{
    class DrinkModel
    {
        public Drink? getDrinkById(int drinkId)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string getDrinkQuery = @" SELECT D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL,
                                                 B.BrandId, B.BrandName, 
                                                 STRING_AGG(C.CategoryId, ',') WITHIN GROUP (ORDER BY C.CategoryId) AS CategoryIds, 
                                                 STRING_AGG(C.CategoryName, ',') WITHIN GROUP (ORDER BY C.CategoryId) AS CategoryNames
                                                 FROM Drink AS D
                                                 LEFT JOIN Brand AS B ON D.BrandId = B.BrandId
                                                 LEFT JOIN DrinkCategory AS DC ON D.DrinkId = DC.DrinkId
                                                 LEFT JOIN Category AS C ON DC.CategoryId = C.CategoryId
                                                 WHERE D.DrinkId = @DrinkId
                                                 GROUP BY D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL, B.BrandId, B.BrandName;";

                List<SqlParameter> queryParameters = new List<SqlParameter>
                {
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId }
                };

                System.Diagnostics.Debug.WriteLine($"Executing query: {getDrinkQuery}");
                foreach (var param in queryParameters)
                {
                    System.Diagnostics.Debug.WriteLine($"Parameter: {param.ParameterName} = {param.Value}");
                }

                var drinkQueryResult = dbService.ExecuteSelect(getDrinkQuery, queryParameters);

                if (drinkQueryResult != null && drinkQueryResult.Count > 0)
                {
                    var row = drinkQueryResult[0];
                    int fetchedDrinkId = Convert.ToInt32(row["DrinkId"]);
                    float alcoholContent = Convert.ToSingle(row["AlcoholContent"]);
                    string drinkName = row["DrinkName"].ToString();
                    string drinkURL = row["DrinkURL"].ToString();

                    int brandId = Convert.ToInt32(row["BrandId"]);
                    string brandName = row["BrandName"].ToString();
                    Brand brand = new Brand(brandId, brandName);

                    List<Category> categories = new List<Category>();
                    if (row["CategoryIds"] != DBNull.Value && row["CategoryNames"] != DBNull.Value)
                    {
                        string[] categoryIds = row["CategoryIds"].ToString().Split(',');
                        string[] categoryNames = row["CategoryNames"].ToString().Split(',');

                        for (int i = 0; i < categoryIds.Length; i++)
                        {
                            if (int.TryParse(categoryIds[i], out int categoryId))
                            {
                                string categoryName = categoryNames[i];
                                categories.Add(new Category(categoryId, categoryName));
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"Warning: Could not parse CategoryId: {categoryIds[i]}");
                            }
                        }
                    }

                    Drink drink = new Drink(fetchedDrinkId, drinkName, drinkURL, categories, brand, alcoholContent);
                    return drink;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in getDrinkById: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception("Database error occurred while getting drink by ID", ex);
            }
        }

        public List<Drink> getDrinks(string? searchedTerm, List<string>? brandNameFilter, List<string>? categoryFilter, float? minAlcohol, float? maxAlcohol, Dictionary<string, bool>? orderBy)
        {
            var dbService = DatabaseService.Instance;
            List<Drink> drinks = new List<Drink>();

            try
            {
                string getDrinksQuery = @" SELECT D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL,
                                  B.BrandId, B.BrandName, 
                                  STRING_AGG(C.CategoryId, ',') WITHIN GROUP (ORDER BY C.CategoryId) AS CategoryIds, 
                                  STRING_AGG(C.CategoryName, ',') WITHIN GROUP (ORDER BY C.CategoryId) AS CategoryNames
                                  FROM Drink AS D
                                  LEFT JOIN Brand AS B ON D.BrandId = B.BrandId
                                  LEFT JOIN DrinkCategory AS DC ON D.DrinkId = DC.DrinkId
                                  LEFT JOIN Category AS C ON DC.CategoryId = C.CategoryId
                                  ";

                List<string> queryConditions = new List<string>();
                List<SqlParameter> queryParameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(searchedTerm))
                {
                    List<string> searchTerms = searchedTerm.Split(' ').ToList();

                    for (int i = 0; i < searchTerms.Count; i++)
                    {
                        string parameterName = $"@SearchTerm{i}";
                        queryConditions.Add($"(LOWER(B.BrandName) LIKE {parameterName} OR LOWER(C.CategoryName) LIKE {parameterName} OR LOWER(D.DrinkName) LIKE {parameterName})");
                        queryParameters.Add(new SqlParameter(parameterName, SqlDbType.VarChar) { Value = "%" + searchTerms[i].ToLower() + "%" });
                    }
                }

                if (brandNameFilter != null && brandNameFilter.Count > 0)
                {
                    var brandParams = brandNameFilter.Select((b, i) => $"@Brand{i}").ToList();
                    queryConditions.Add($"LOWER(LTRIM(RTRIM(B.BrandName))) IN ({string.Join(", ", brandParams)})");

                    for (int i = 0; i < brandNameFilter.Count; i++)
                    {
                        queryParameters.Add(new SqlParameter($"@Brand{i}", SqlDbType.VarChar) { Value = brandNameFilter[i].Trim().ToLower() });
                    }
                }

                if (categoryFilter != null && categoryFilter.Count > 0)
                {
                    var categoryParams = categoryFilter.Select((c, i) => $"@Category{i}").ToList();
                    queryConditions.Add($"LOWER(LTRIM(RTRIM(C.CategoryName))) IN ({string.Join(", ", categoryParams)})");

                    for (int i = 0; i < categoryFilter.Count; i++)
                    {
                        queryParameters.Add(new SqlParameter($"@Category{i}", SqlDbType.VarChar) { Value = categoryFilter[i].Trim().ToLower() });
                    }
                }

                if (minAlcohol.HasValue)
                {
                    queryConditions.Add("D.AlcoholContent >= @MinAlcohol");
                    queryParameters.Add(new SqlParameter("@MinAlcohol", SqlDbType.Float) { Value = minAlcohol.Value });
                }

                if (maxAlcohol.HasValue)
                {
                    queryConditions.Add("D.AlcoholContent <= @MaxAlcohol");
                    queryParameters.Add(new SqlParameter("@MaxAlcohol", SqlDbType.Float) { Value = maxAlcohol.Value });
                }

                if (queryConditions.Any())
                {
                    getDrinksQuery += " WHERE " + string.Join(" AND ", queryConditions);
                }

                getDrinksQuery += " GROUP BY D.DrinkId, D.AlcoholContent, D.DrinkName, D.DrinkURL, B.BrandId, B.BrandName HAVING COUNT(DISTINCT B.BrandName) > 0";

                if (orderBy != null && orderBy.Count > 0)
                {
                    var orderClauses = orderBy.Select(o => $"{o.Key} {(o.Value ? "ASC" : "DESC")}");
                    getDrinksQuery += " ORDER BY " + string.Join(", ", orderClauses);
                }

                var drinkQueryResult = dbService.ExecuteSelect(getDrinksQuery, queryParameters);

                foreach (var row in drinkQueryResult)
                {
                    int drinkId = Convert.ToInt32(row["DrinkId"]);
                    float alcoholContent = Convert.ToSingle(row["AlcoholContent"]);
                    string drinkName = row["DrinkName"].ToString();
                    string drinkURL = row["DrinkURL"].ToString();

                    int brandId = Convert.ToInt32(row["BrandId"]);
                    string brandName = row["BrandName"].ToString();
                    Brand brand = new Brand(brandId, brandName);

                    List<Category> categories = new List<Category>();
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

                List<SqlParameter> brandNameParameter =
                [
                    new SqlParameter("@BrandName", SqlDbType.VarChar) { Value = brandName }
                ];
                var brandIdResult = dbService.ExecuteSelect(brandIdQuery, brandNameParameter);
                int brandId = brandIdResult.Count > 0 ? Convert.ToInt32(brandIdResult[0]["BrandId"]) : -1;

                if (brandId == -1)
                {
                    string addBrandQuery = @"INSERT INTO Brand (BrandName) VALUES (@BrandName);
                                          SELECT SCOPE_IDENTITY() AS BrandId;";
                    List<SqlParameter> brandParameters =
                    [
                        new SqlParameter("@BrandName", SqlDbType.VarChar) { Value = brandName }
                    ];

                    var newBrandResult = dbService.ExecuteSelect(addBrandQuery, brandParameters);
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

                var drinkIdResult = dbService.ExecuteSelect(addDrinkQuery, drinkParameters);
                int drinkId = Convert.ToInt32(drinkIdResult[0]["DrinkId"]);

                foreach (Category category in categories)
                {
                    string addCategoriesQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) 
                                               VALUES (@DrinkId, @CategoryId);";
                    List<SqlParameter> categoryParameters =
                    [
                        new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId },
                        new SqlParameter("@CategoryId", SqlDbType.Int) { Value = category.Id }
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
                List<SqlParameter> drinkIdParameter =
                [
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId }
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
                List<SqlParameter> brandNameParameter =
                [
                    new SqlParameter("@BrandName", SqlDbType.VarChar) { Value = drink.Brand.Name }
                ];
                var brandIdResult = dbService.ExecuteSelect(brandIdQuery, brandNameParameter);
                int brandId = brandIdResult.Count > 0 ? Convert.ToInt32(brandIdResult[0]["BrandId"]) : -1;
                if (brandId == -1)
                {
                    throw new Exception("Brand does not exist");
                }

                string updateDrinkQuery = @"UPDATE Drink SET AlcoholContent = @AlcoholContent, BrandId = @BrandId, DrinkName = @DrinkName, DrinkUrl = @DrinkUrl
                                            WHERE DrinkId = @DrinkId;";
                List<SqlParameter> drinkParameters =
                [
                    new SqlParameter("@DrinkName", SqlDbType.VarChar) { Value = drink.DrinkName },
                    new SqlParameter("@DrinkUrl", SqlDbType.VarChar) { Value = drink.DrinkURL },
                    new SqlParameter("@AlcoholContent", SqlDbType.Float) { Value = drink.AlcoholContent },
                    new SqlParameter("@BrandId", SqlDbType.Int) { Value = brandId },
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drink.Id }
                ];
                dbService.ExecuteQuery(updateDrinkQuery, drinkParameters);

                string getExistingCategoriesQuery = @"SELECT CategoryId FROM DrinkCategory 
                                                    WHERE DrinkId = @DrinkId";
                List<SqlParameter> drinkIdParam =
                [
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drink.Id }
                ];
                var existingCategoriesResult = dbService.ExecuteSelect(getExistingCategoriesQuery, drinkIdParam);
                HashSet<int> existingCategories = [.. existingCategoriesResult.Select(row => Convert.ToInt32(row["CategoryId"]))];
                HashSet<int> newCategories = [.. drink.Categories.Select(c => c.Id)];

                var categoriesToInsert = newCategories.Except(existingCategories).ToList();
                var categoriesToDelete = existingCategories.Except(newCategories).ToList();

                foreach (int categoryId in categoriesToInsert)
                {
                    string addCategoriesQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) 
                                               VALUES (@DrinkId, @CategoryId);";
                    List<SqlParameter> categoryParameters =
                    [
                        new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drink.Id },
                        new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId }
                    ];
                    dbService.ExecuteQuery(addCategoriesQuery, categoryParameters);
                }

                foreach (int categoryId in categoriesToDelete)
                {
                    string deleteCategoriesQuery = @"DELETE FROM DrinkCategory 
                                                 WHERE DrinkId = @DrinkId AND CategoryId = @CategoryId";
                    List<SqlParameter> categoryParameters =
                    [
                        new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drink.Id },
                        new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId }
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

            string query = @"
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
                ORDER BY d.DrinkId";
            // Use TOP instead of LIMIT for SQL Server
            // OFFSET-FETCH can be used for pagination if needed

            var parameters = new List<SqlParameter>
            {
                new("@UserId", SqlDbType.Int) { Value = userId },
                new("@NumberOfDrinks", SqlDbType.Int) { Value = numberOfDrinks }
            };

            try
            {
                var selectResult = dbService.ExecuteSelect(query, parameters);
                var drinks = new List<Drink>();

                foreach (var row in selectResult)
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

        public bool isDrinkInPersonalList(int userId, int drinkId)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string checkQuery = "SELECT COUNT(*) AS DrinkCount FROM UserDrink WHERE UserId = @UserId AND DrinkId = @DrinkId;";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId }
                };

                var result = dbService.ExecuteSelect(checkQuery, parameters);

                if (result != null && result.Count > 0)
                {
                    int count = Convert.ToInt32(result[0]["DrinkCount"]);
                    return count > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error checking if drink is in personal list: {ex.Message}");
                return false;
            }
        }

        public bool addToPersonalDrinkList(int userId, int drinkId)
        {
            var dbService = DatabaseService.Instance;

            try
            {
                string insertQuery = "INSERT INTO UserDrink (UserId, DrinkId) VALUES (@UserId, @DrinkId);";

                List<SqlParameter> parameters =
                [
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId }
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

                List<SqlParameter> parameters =
                [
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId }
                ];

                int rowsAffected = dbService.ExecuteQuery(deleteQuery, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete drink from personal list", ex);
            }
        }

        public void voteDrinkOfTheDay(int drinkId, int userId)
        {
            var dbService = DatabaseService.Instance;
            DateTime voteTime = DateTime.UtcNow;

            try
            {
                string checkQuery = @"
                SELECT COUNT(*) AS VoteCount FROM Vote 
                WHERE UserId = @UserId 
                AND CONVERT(date, VoteTime) = CONVERT(date, @VoteTime);";

                var checkParams = new List<SqlParameter>
                {
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@VoteTime", SqlDbType.DateTime) { Value = voteTime }
                };

                var result = dbService.ExecuteSelect(checkQuery, checkParams);
                int count = Convert.ToInt32(result[0]["VoteCount"]);

                if (count > 0)
                {
                    string updateQuery = @"
                    UPDATE Vote 
                    SET DrinkId = @DrinkId, VoteTime = @VoteTime 
                    WHERE UserId = @UserId 
                    AND CONVERT(date, VoteTime) = CONVERT(date, @VoteTime);";

                    var updateParams = new List<SqlParameter>
                    {
                        new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId },
                        new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                        new SqlParameter("@VoteTime", SqlDbType.DateTime) { Value = voteTime }
                    };

                    dbService.ExecuteQuery(updateQuery, updateParams);
                }
                else
                {
                    string insertQuery = @"
                    INSERT INTO Vote (UserId, DrinkId, VoteTime) 
                    VALUES (@UserId, @DrinkId, @VoteTime);";

                    var insertParams = new List<SqlParameter>
                    {
                        new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId },
                        new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                        new SqlParameter("@VoteTime", SqlDbType.DateTime) { Value = voteTime }
                    };

                    dbService.ExecuteQuery(insertQuery, insertParams);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to vote for drink of the day", ex);
            }
        }

        public Drink getDrinkOfTheDay()
        {
            var dbService = DatabaseService.Instance;
            try
            {
                string getDrinkOfTheDayQuery = "SELECT * FROM DrinkOfTheDay;";
                var drinkQueryResult = dbService.ExecuteSelect(getDrinkOfTheDayQuery);

                if (drinkQueryResult.Count == 0)
                {
                    setDrinkOfTheDay();
                    drinkQueryResult = dbService.ExecuteSelect(getDrinkOfTheDayQuery);
                }

                int drinkId = Convert.ToInt32(drinkQueryResult[0]["DrinkId"]);
                DateTime drinkOfTheDaySetTime = Convert.ToDateTime(drinkQueryResult[0]["DrinkTime"]);

                if (drinkOfTheDaySetTime.Date != DateTime.UtcNow.Date)
                {
                    resetDrinkOfTheDay();
                    drinkQueryResult = dbService.ExecuteSelect(getDrinkOfTheDayQuery);
                    drinkId = Convert.ToInt32(drinkQueryResult[0]["DrinkId"]);
                }

                string getDrinkQuery = "SELECT D.BrandId, D.DrinkName, D.DrinkURL, D.AlcoholContent FROM Drink AS D WHERE D.DrinkId = @DrinkId;";
                List<SqlParameter> drinkIdParameter =
                [
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = drinkId }
                ];
                drinkQueryResult = dbService.ExecuteSelect(getDrinkQuery, drinkIdParameter);

                if (drinkQueryResult.Count == 0)
                {
                    throw new Exception("No drink of the day with id " + drinkId + " found");
                }

                int brandId = Convert.ToInt32(drinkQueryResult[0]["BrandId"]);
                string drinkName = drinkQueryResult[0]["DrinkName"].ToString();
                string drinkURL = drinkQueryResult[0]["DrinkURL"].ToString();
                float alcoholContent = (float)Convert.ToDouble(drinkQueryResult[0]["AlcoholContent"]);
                string getCategoriesQuery = "SELECT C.CategoryId, C.CategoryName FROM Category AS C JOIN DrinkCategory AS DC ON DC.CategoryId = C.CategoryId WHERE DC.DrinkId = @DrinkId;";
                var categoryQueryResult = dbService.ExecuteSelect(getCategoriesQuery, drinkIdParameter);

                List<Category> categories = [];
                foreach (var row in categoryQueryResult)
                {
                    int categoryId = Convert.ToInt32(row["CategoryId"]);
                    string categoryName = row["CategoryName"].ToString();
                    categories.Add(new Category(categoryId, categoryName));
                }

                string getBrandQuery = "SELECT B.BrandId, B.BrandName FROM Brand AS B WHERE B.BrandId = @BrandId;";
                List<SqlParameter> brandIdParameter =
                [
                    new SqlParameter("@BrandId", SqlDbType.Int) { Value = brandId }
                ];
                var brandQueryResult = dbService.ExecuteSelect(getBrandQuery, brandIdParameter);

                if (brandQueryResult.Count == 0)
                {
                    throw new Exception("Brand not found");
                }

                string brandName = brandQueryResult[0]["BrandName"].ToString();
                Brand brand = new Brand(brandId, brandName);

                Drink drinkOfTheDay = new Drink(drinkId, drinkName, drinkURL, categories, brand, alcoholContent);
                return drinkOfTheDay;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get drink of the day", ex);
            }
        }

        private int getCurrentTopVotedDrink()
        {
            var dbService = DatabaseService.Instance;
            string topVoteCountQuery = @"
                SELECT TOP 1 DrinkId, COUNT(*) AS VoteCount 
                FROM Vote 
                WHERE CONVERT(date, VoteTime) = CONVERT(date, @VoteTime)
                GROUP BY DrinkId
                ORDER BY VoteCount DESC;";

            List<SqlParameter> voteDayParameter =
            [
                new SqlParameter("@VoteTime", SqlDbType.DateTime) { Value = DateTime.UtcNow.Date.AddDays(-1) }
            ];

            var topVoteCountResult = dbService.ExecuteSelect(topVoteCountQuery, voteDayParameter);

            if (topVoteCountResult.Count == 0)
            {
                return getRandomDrinkId();
            }

            return Convert.ToInt32(topVoteCountResult[0]["DrinkId"]);
        }

        public int getRandomDrinkId()
        {
            var dbService = DatabaseService.Instance;
            try
            {
                // SQL Server uses NEWID() for random ordering
                string getRandomDrinkIdQuery = "SELECT TOP 1 DrinkId FROM Drink ORDER BY NEWID();";
                var selectResult = dbService.ExecuteSelect(getRandomDrinkIdQuery);
                if (selectResult.Count > 0)
                {
                    return Convert.ToInt32(selectResult[0]["DrinkId"]);
                }
                else
                {
                    throw new Exception("No drinks found in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get random drink ID", ex);
            }
        }

        private void setDrinkOfTheDay()
        {
            var dbService = DatabaseService.Instance;
            try
            {
                int newDrinkOfTheDayId = getCurrentTopVotedDrink();

                string insertDrinkOfTheDayQuery = "INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime) VALUES (@DrinkId, @DrinkTime);";
                List<SqlParameter> parameters =
                [
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = newDrinkOfTheDayId },
                    new SqlParameter("@DrinkTime", SqlDbType.DateTime) { Value = DateTime.UtcNow }
                ];

                dbService.ExecuteQuery(insertDrinkOfTheDayQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to set drink of the day", ex);
            }
        }

        private void resetDrinkOfTheDay()
        {
            var dbService = DatabaseService.Instance;
            try
            {
                int newDrinkOfTheDayId = getCurrentTopVotedDrink();
                string updateDrinkOfTheDayQuery = "UPDATE DrinkOfTheDay SET DrinkId = @DrinkId, DrinkTime = @DrinkTime;";
                List<SqlParameter> parameters =
                [
                    new SqlParameter("@DrinkId", SqlDbType.Int) { Value = newDrinkOfTheDayId },
                    new SqlParameter("@DrinkTime", SqlDbType.DateTime) { Value = DateTime.UtcNow }
                ];

                dbService.ExecuteQuery(updateDrinkOfTheDayQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to reset drink of the day", ex);
            }
        }
    }
}
