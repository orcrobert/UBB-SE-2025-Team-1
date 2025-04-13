using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using WinUIApp.Database;
using Microsoft.Data.SqlClient; // Ensure you have the correct using directive for SQL Server

namespace WinUIApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private static DatabaseService _databaseServiceInstance;
        private static readonly object _databaseServiceInstanceLock = new();
        private readonly DatabaseConnection _databaseConnection;


        public static DatabaseService Instance
        {
            get
            {
                if (_databaseServiceInstance == null)
                {
                    lock (_databaseServiceInstanceLock)
                    {
                        if (_databaseServiceInstance == null)
                        {
                            lock (_databaseServiceInstanceLock)
                            {
                                _databaseServiceInstance = new DatabaseService();
                            }
                        }
                    }
                }
                return _databaseServiceInstance;
            }
        }

        private DatabaseService()
        {
            _databaseConnection = DatabaseConnection.Instance;
        }

        public List<Dictionary<string, object>> ExecuteSelectQuery(string sqlSelectQuery, List<SqlParameter> sqlSelectQueryParameters = null)
        {
            var selectQueryResults = new List<Dictionary<string, object>>();

            try
            {
                _databaseConnection.OpenConnection();
                using var sqlSelectCommand = new SqlCommand(sqlSelectQuery, _databaseConnection.GetConnection());
                if (sqlSelectQueryParameters != null)
                {
#pragma warning disable IDE0305
                    sqlSelectCommand.Parameters.AddRange(sqlSelectQueryParameters.ToArray());
#pragma warning restore IDE0305

                }

                using var sqlDataReader = sqlSelectCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    var queryResultRowData = new Dictionary<string, object>();
                    for (int currentColumnIndex = 0; currentColumnIndex < sqlDataReader.FieldCount; currentColumnIndex++)
                    {
                        queryResultRowData[sqlDataReader.GetName(currentColumnIndex)] = sqlDataReader.GetValue(currentColumnIndex);
                    }
                    selectQueryResults.Add(queryResultRowData);
                }
            }
            catch (Exception executeSelectQueryException)
            {
                Debug.WriteLine($"Error executing select query: {executeSelectQueryException.Message}");
            }
            finally
            {
                _databaseConnection.CloseConnection();
            }

            return selectQueryResults;
        }

        public int ExecuteDataModificationQuery(string sqlDataModificationQuery, List<SqlParameter> sqlDataModificationQueryParameters = null)
        {
            int numberOfRowsAffectedByQuery = 0;

            try
            {
                _databaseConnection.OpenConnection();
                using var dataModificationQueryCommand = new SqlCommand(sqlDataModificationQuery, _databaseConnection.GetConnection());
                if (sqlDataModificationQueryParameters != null)
                {
#pragma warning disable IDE0305
                    dataModificationQueryCommand.Parameters.AddRange(sqlDataModificationQueryParameters.ToArray());
#pragma warning restore IDE0305

                }
                numberOfRowsAffectedByQuery = dataModificationQueryCommand.ExecuteNonQuery();
            }
            catch (Exception dataModificationQueryExecutionException)
            {
                Debug.WriteLine($"Error executing data modification query: {dataModificationQueryExecutionException.Message}");
            }
            finally
            {
                _databaseConnection.CloseConnection();
            }

            return numberOfRowsAffectedByQuery;
        }
    }
}