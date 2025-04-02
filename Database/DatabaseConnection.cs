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

        private DatabaseConnection()
        {
            try
            {
                _connection = new MySqlConnection(_connectionString);
                Debug.WriteLine("DatabaseConnection constructor: Connection created.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DatabaseConnection constructor error: {ex.Message}");
            }
        }


        public static DatabaseConnection Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null) 
                    {   
                        lock(_lock)
                        {
                            _instance = new DatabaseConnection();
                        }
                    }
                    return _instance;
                }
            }
        }

        public MySqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new MySqlConnection(_connectionString);
            }

            return _connection;
        }


        public void OpenConnection()
        {
            try
            {
                var connection = GetConnection();

                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
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
                var connection = GetConnection();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error closing connection: {ex.Message}");
            }
        }

    }
}
