using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WinUIApp.Services
{
    public class DatabaseService
    {
        private static DatabaseService _instance;
        private static readonly object _lock = new object();
        private MySqlConnection _connection;
        private static string _connectionString;

        static DatabaseService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
        }

        private DatabaseService()
        {
            _connection = new MySqlConnection(_connectionString);
        }

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
                            _instance = new DatabaseService();
                        }
                    }
                }
                return _instance;
            }
        }

        public MySqlConnection GetConnection()
        {
            return _connection;
        }

        public void OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public List<Dictionary<string, object>> ExecuteSelect(string query, List<MySqlParameter> parameters = null)
        {
            var result = new List<Dictionary<string, object>>();

            try
            {
                OpenConnection();

                using (var command = new MySqlCommand(query, _connection))
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
                Console.WriteLine($"Error executing query: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }

        public int ExecuteQuery(string query, List<MySqlParameter> parameters = null)
        {
            int rowsAffected = 0;

            try
            {
                OpenConnection();

                using (var command = new MySqlCommand(query, _connection))
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
                Console.WriteLine($"Error executing query: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return rowsAffected;
        }
    }
}
