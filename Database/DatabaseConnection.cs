using System;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;

namespace WinUIApp.Database
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private static readonly object _lock = new object();
        private MySqlConnection _connection;
        private static string _connectionString;

        static DatabaseConnection()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
        }

        public DatabaseConnection()
        {
            _connection = new MySqlConnection(_connectionString);
        }

        public static DatabaseConnection Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseConnection();
                    }
                    return _instance;
                }
            }
        }

        public MySqlConnection GetConnection()
        {
            return _connection;
        }

        public void OpenConnection()
        {
            try
            {
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening connection: {ex.Message}");
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error closing connection: {ex.Message}");
            }
        }
    }
}
