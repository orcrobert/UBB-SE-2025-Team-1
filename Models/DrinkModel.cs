using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIApp.Services;

namespace WinUIApp.Models
{
    class DrinkModel
    {
        public List<Drink> getDrinks()
        {
            var dbService = DatabaseService.Instance;
            List<Drink> drinks = new List<Drink>();

            try
            {
                string getDrinksQuery = @" SELECT D.DrinkId, D.AlcoholContent, 
                                         B.BrandId, B.BrandName, 
                                         GROUP_CONCAT(C.CategoryId ORDER BY C.CategoryId) AS CategoryIds, 
                                         GROUP_CONCAT(C.CategoryName ORDER BY C.CategoryId) AS CategoryNames
                                    FROM Drink AS D
                                    INNER JOIN Brand AS B ON D.BrandId = B.BrandId
                                    LEFT JOIN DrinkCategory AS DC ON D.DrinkId = DC.DrinkId
                                    LEFT JOIN Category AS C ON DC.CategoryId = C.CategoryId
                                    GROUP BY D.DrinkId, B.BrandId
                                    ORDER BY D.DrinkId;";

                var selectResult = dbService.ExecuteSelect(getDrinksQuery);


                foreach (var row in selectResult)
                {
                    int drinkId = Convert.ToInt32(row["DrinkId"]);
                    float alcoholContent = Convert.ToSingle(row["AlcoholContent"]);

                    int brandId = Convert.ToInt32(row["BrandId"]);
                    string brandName = row["BrandName"].ToString();
                    Brand brand = new Brand(brandId, brandName);

                    List<Category> categories = new List<Category>();

                    if (row["CategoryIds"] != DBNull.Value)
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

                    Drink drink = new Drink(drinkId, categories, brand, alcoholContent);
                    drinks.Add(drink);
                }

                return drinks;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }


        public void addDrink(List<Category> categories, string brandName, float alcoholContent)
        {
            var dbService = DatabaseService.Instance;
            try
            {
                string brandIdQuery = @"SELECT BrandId 
                                        FROM Brand 
                                        WHERE BrandName = @BrandName";

                List<MySqlParameter> brandNameParameter = new List<MySqlParameter>
                {
                    new MySqlParameter("@BrandName", MySqlDbType.VarChar) { Value = brandName }
                };
                var brandIdResult = dbService.ExecuteSelect(brandIdQuery, brandNameParameter);
                int brandId = brandIdResult.Count > 0 ? Convert.ToInt32(brandIdResult[0]["BrandId"]) : -1;


                if (brandId == -1)
                {
                    throw new Exception("Brand does not exist");
                }

                string addDrinkQuery = @"INSERT INTO Drink (AlcoholContent, BrandId) 
                                VALUES (@AlcoholContent, @BrandId);";

                List<MySqlParameter> drinkParameters = new List<MySqlParameter>
                {
                    new MySqlParameter("@AlcoholContent", MySqlDbType.Float) { Value = alcoholContent },
                    new MySqlParameter("@BrandId", MySqlDbType.Int32) { Value = brandId }
                };

                dbService.ExecuteQuery(addDrinkQuery, drinkParameters);


                string lastDrinkIdQuery = "SELECT LAST_INSERT_ID() AS DrinkId";
                var lastDrinkIdResult = dbService.ExecuteSelect(lastDrinkIdQuery);
                int drinkId = Convert.ToInt32(lastDrinkIdResult[0]["DrinkId"]);

                foreach (Category category in categories)
                {
                    string addCategoriesQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) 
                                               VALUES (@DrinkId, @CategoryId);";
                    List<MySqlParameter> categoryParameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId },
                        new MySqlParameter("@CategoryId", MySqlDbType.Int32) { Value = category.Id }
                    };

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
                List<MySqlParameter> drinkIdParameter = new List<MySqlParameter>
                {
                    new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId }
                };
                dbService.ExecuteQuery(deleteDrinkQuery, drinkIdParameter);
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }

        public void updateDrink(int drinkId, List<Category> categories, string brandName, float alcoholContent)
        {
            var dbService = DatabaseService.Instance;
            try
            {
                string brandidQuery = @"SELECT BrandId FROM Brand 
                                        WHERE BrandName = @BrandName";
                List<MySqlParameter> brandNameParameter = new List<MySqlParameter>
                {
                    new MySqlParameter("@BrandName", MySqlDbType.VarChar) { Value = brandName }
                };
                var brandIdResult = dbService.ExecuteSelect(brandidQuery, brandNameParameter);
                int brandId = brandIdResult.Count > 0 ? Convert.ToInt32(brandIdResult[0]["BrandId"]) : -1;
                if (brandId == -1)
                {
                    throw new Exception("Brand does not exist");
                }


                string updateDrinkQuery = @"UPDATE Drink SET AlcoholContent = @AlcoholContent, BrandId = @BrandId 
                                            WHERE DrinkId = @DrinkId;";
                List<MySqlParameter> drinkParameters = new List<MySqlParameter>
                {
                    new MySqlParameter("@AlcoholContent", MySqlDbType.Float) { Value = alcoholContent },
                    new MySqlParameter("@BrandId", MySqlDbType.Int32) { Value = brandId },
                    new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId }
                };
                dbService.ExecuteQuery(updateDrinkQuery, drinkParameters);


                string getExistingCategoriesQuery = @"SELECT CategoryId FROM DrinkCategory 
                                                    WHERE DrinkId = @DrinkId";
                var existingCategoriesResult = dbService.ExecuteSelect(getExistingCategoriesQuery, drinkParameters);
                HashSet<int> existingCategories = new HashSet<int>(existingCategoriesResult.Select(row => Convert.ToInt32(row["CategoryId"])));
                HashSet<int> newCategories = new HashSet<int>(categories.Select(c => c.Id));

                var categoriesToInsert = newCategories.Except(existingCategories).ToList();
                var categoriesToDelete = existingCategories.Except(newCategories).ToList();

                foreach (int categoryId in categoriesToInsert)
                {
                    string addCategoriesQuery = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) 
                                               VALUES (@DrinkId, @CategoryId);";
                    List<MySqlParameter> categoryParameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId },
                        new MySqlParameter("@CategoryId", MySqlDbType.Int32) { Value = categoryId }
                    };
                    dbService.ExecuteQuery(addCategoriesQuery, categoryParameters);
                }

                foreach (int categoryId in categoriesToDelete)
                {
                    string deleteCategoriesQuery = @"DELETE FROM DrinkCategory 
                                                 WHERE DrinkId = @DrinkId AND CategoryId = @CategoryId";
                    List<MySqlParameter> categoryParameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId },
                        new MySqlParameter("@CategoryId", MySqlDbType.Int32) { Value = categoryId }
                    };
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

                List<Category> categories = new List<Category>();
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
                List<Brand> brands = new List<Brand>();
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


                List<MySqlParameter> parameters = new List<MySqlParameter>
                {
                    new MySqlParameter("@UserId", MySqlDbType.Int32) { Value = userId },
                    new MySqlParameter("@NumbersOfDrinks", MySqlDbType.Int32) { Value = numberOfDrinks },
                };

                var selectResult = dbService.ExecuteSelect(getPersonalDrinkList, parameters);
                List<Drink> drinks = new List<Drink>();

                foreach (var row in selectResult)
                {
                    int drinkId = Convert.ToInt32(row["DrinkId"]);
                    float alcoholContent = (float)(row["AlcoholContent"]);

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
                            categories.Add(new Category(Convert.ToInt32(categoryIds[i]), categoryNames[i]));
                        }
                    }

                    Drink drink = new Drink(drinkId, categories, brand, alcoholContent);
                    drinks.Add(drink);
                }
                return drinks;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }
    }
}
