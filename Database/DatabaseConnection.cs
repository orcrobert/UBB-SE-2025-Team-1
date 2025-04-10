using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Data.SqlClient; // Ensure you have the correct using directive for SQL Server

namespace WinUIApp.Database
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private static readonly object _lock = new object();
        private SqlConnection _connection;
        private static string _connectionString;

        static DatabaseConnection()
        {
            _connectionString = "Data Source=DESKTOP-51TFR1B;Initial Catalog=ISSApp;Integrated Security=True;TrustServerCertificate=True";
        }

        private DatabaseConnection()
        {
            try
            {
                _connection = new SqlConnection(_connectionString);
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

        public SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
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
