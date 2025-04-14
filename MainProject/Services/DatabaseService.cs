// <copyright file="DatabaseService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.Data.SqlClient;
    using WinUIApp.Database;

    /// <summary>
    /// Service for managing database operations.
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        private static readonly object DatabaseServiceInstanceLock = new ();
        private static DatabaseService databaseServiceInstance;
        private readonly DatabaseConnection databaseConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseService"/> class.
        /// </summary>
        private DatabaseService()
        {
            this.databaseConnection = DatabaseConnection.Instance;
        }

        /// <summary>
        /// Gets the singleton instance of the DatabaseService class.
        /// </summary>
        public static DatabaseService Instance
        {
            get
            {
                if (databaseServiceInstance == null)
                {
                    lock (DatabaseServiceInstanceLock)
                    {
                        if (databaseServiceInstance == null)
                        {
                            lock (DatabaseServiceInstanceLock)
                            {
                                databaseServiceInstance = new DatabaseService();
                            }
                        }
                    }
                }

                return databaseServiceInstance;
            }
        }

        /// <summary>
        /// Executes a SQL SELECT query and returns the results as a list of dictionaries.
        /// </summary>
        /// <param name="sqlSelectQuery"> Query. </param>
        /// <param name="sqlSelectQueryParameters"> Parameters. </param>
        /// <returns> Result. </returns>
        public List<Dictionary<string, object>> ExecuteSelectQuery(string sqlSelectQuery, List<SqlParameter> sqlSelectQueryParameters = null)
        {
            var selectQueryResults = new List<Dictionary<string, object>>();

            try
            {
                this.databaseConnection.OpenConnection();
                using var sqlSelectCommand = new SqlCommand(sqlSelectQuery, this.databaseConnection.GetConnection());
                if (sqlSelectQueryParameters != null)
                {
                    sqlSelectCommand.Parameters.AddRange(sqlSelectQueryParameters.ToArray());
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
                this.databaseConnection.CloseConnection();
            }

            return selectQueryResults;
        }

        /// <summary>
        /// Executes a SQL data modification query (INSERT, UPDATE, DELETE) and returns the number of rows affected.
        /// </summary>
        /// <param name="sqlDataModificationQuery"> Query. </param>
        /// <param name="sqlDataModificationQueryParameters"> Parameters. </param>
        /// <returns> Result. </returns>
        public int ExecuteDataModificationQuery(string sqlDataModificationQuery, List<SqlParameter> sqlDataModificationQueryParameters = null)
        {
            int numberOfRowsAffectedByQuery = 0;

            try
            {
                this.databaseConnection.OpenConnection();
                using var dataModificationQueryCommand = new SqlCommand(sqlDataModificationQuery, this.databaseConnection.GetConnection());
                if (sqlDataModificationQueryParameters != null)
                {
                    dataModificationQueryCommand.Parameters.AddRange(sqlDataModificationQueryParameters.ToArray());
                }

                numberOfRowsAffectedByQuery = dataModificationQueryCommand.ExecuteNonQuery();
            }
            catch (Exception dataModificationQueryExecutionException)
            {
                Debug.WriteLine($"Error executing data modification query: {dataModificationQueryExecutionException.Message}");
            }
            finally
            {
                this.databaseConnection.CloseConnection();
            }

            return numberOfRowsAffectedByQuery;
        }
    }
}