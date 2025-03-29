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
                string query = @"
            SELECT 
                D.DrinkId, D.AlcoholContent, 
                B.BrandId, B.BrandName, 
                GROUP_CONCAT(C.CategoryId ORDER BY C.CategoryId) AS CategoryIds, 
                GROUP_CONCAT(C.CategoryName ORDER BY C.CategoryId) AS CategoryNames
            FROM Drink AS D
            INNER JOIN Brand AS B ON D.BrandId = B.BrandId
            LEFT JOIN DrinkCategory AS DC ON D.DrinkId = DC.DrinkId
            LEFT JOIN Category AS C ON DC.CategoryId = C.CategoryId
            GROUP BY D.DrinkId, B.BrandId
            ORDER BY D.DrinkId;";

                var selectResult = dbService.ExecuteSelect(query);

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
                string brandidQuery = @"SELECT BrandId FROM Brand WHERE BrandName = @BrandName";
                var brandNameParameter = new List<MySqlParameter>
                {
                    new MySqlParameter("@BrandName", MySqlDbType.VarChar) { Value = brandName }
                };
                var brandIdResult = dbService.ExecuteSelect(brandidQuery, brandNameParameter);
                int brandId = brandIdResult.Count > 0 ? Convert.ToInt32(brandIdResult[0]["BrandId"]) : -1;

                if (brandId == -1)
                {
                    throw new Exception("Brand does not exist");
                }

                string query = @"INSERT INTO Drink (AlcoholContent, BrandId) VALUES (@AlcoholContent, @BrandId);";
                var parameters = new List<MySqlParameter>
                {
                    new MySqlParameter("@AlcoholContent", MySqlDbType.Float) { Value = alcoholContent },
                    new MySqlParameter("@BrandId", MySqlDbType.Int32) { Value = brandId }
                };

                dbService.ExecuteQuery(query, parameters);


                string lastDrinkIdQuery = "SELECT LAST_INSERT_ID() AS DrinkId";
                var lastDrinkIdResult = dbService.ExecuteSelect(lastDrinkIdQuery);
                int drinkId = Convert.ToInt32(lastDrinkIdResult[0]["DrinkId"]);

                foreach (var category in categories)
                {
                    query = @"INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (@DrinkId, @CategoryId);";
                    parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("@DrinkId", MySqlDbType.Int32) { Value = drinkId },
                        new MySqlParameter("@CategoryId", MySqlDbType.Int32) { Value = category.Id }
                    };
                    dbService.ExecuteQuery(query, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error occurred", ex);
            }
        }

    }
}
