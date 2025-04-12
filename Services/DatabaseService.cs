using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; // Changed from MySql to SQL Server
using System.Configuration;
using System.Diagnostics;
using WinUIApp.Database;
using Microsoft.Data.SqlClient; // Ensure you have the correct using directive for SQL Server

namespace WinUIApp.Services
{
    public class DatabaseService
    {
        private static DatabaseService _instance;
        private static readonly object _lock = new object();
        private DatabaseConnection _databaseConnection;

        public static DatabaseService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            lock (_lock)
                            {
                                _instance = new DatabaseService();
                            }
                        }
                    }
                }
                return _instance;
            }
        }

        private DatabaseService()
        {
            _databaseConnection = DatabaseConnection.Instance;
        }

        public List<Dictionary<string, object>> ExecuteSelect(string query, List<SqlParameter> parameters = null)
        {
            var result = new List<Dictionary<string, object>>();

            try
            {
                _databaseConnection.OpenConnection();
                using (var command = new SqlCommand(query, _databaseConnection.GetConnection()))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            result.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error executing query: {ex.Message}");
            }
            finally
            {
                _databaseConnection.CloseConnection();
            }

            return result;
        }

        public int ExecuteQuery(string query, List<SqlParameter> parameters = null)
        {
            int rowsAffected = 0;

            try
            {
                _databaseConnection.OpenConnection();
                using (var command = new SqlCommand(query, _databaseConnection.GetConnection()))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error executing query: {ex.Message}");
            }
            finally
            {
                _databaseConnection.CloseConnection();
            }

            return rowsAffected;
        }
    }
}