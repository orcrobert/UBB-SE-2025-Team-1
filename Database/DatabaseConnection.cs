using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Data.SqlClient; // Ensure you have the correct using directive for SQL Server

namespace WinUIApp.Database
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private static readonly object _lock = new();
        private SqlConnection _connection;
        private static readonly string _connectionString;

        static DatabaseConnection()
        {
            _connectionString = "Data Source=sql6032.site4now.net;" +
        "Initial Catalog=db_aaae7e_beatrice;" +
        "User ID=db_aaae7e_beatrice_admin;" +
        "Password=bea12345;" +
        "Encrypt=True;" +
        "TrustServerCertificate=True;";
        }



        public static DatabaseConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new DatabaseConnection();
                    }
                }
                return _instance;
            }
        }

        public SqlConnection GetConnection()
        {
            if (_connection == null || _connection.State == System.Data.ConnectionState.Broken)
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
