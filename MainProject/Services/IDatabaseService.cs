// <copyright file="IDatabaseService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Services
{
    using System.Collections.Generic;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Interface for managing database operations.
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Gets the singleton instance of the DatabaseService class.
        /// </summary>
        static abstract DatabaseService Instance { get; }

        /// <summary>
        /// Executes a SQL data modification query (INSERT, UPDATE, DELETE) and returns the number of rows affected.
        /// </summary>
        /// <param name="sqlDataModificationQuery"> Query. </param>
        /// <param name="sqlDataModificationQueryParameters"> Parameters. </param>
        /// <returns> Result. </returns>
        int ExecuteDataModificationQuery(string sqlDataModificationQuery, List<SqlParameter> sqlDataModificationQueryParameters = null);

        /// <summary>
        /// Executes a SQL SELECT query and returns the results as a list of dictionaries.
        /// </summary>
        /// <param name="sqlSelectQuery"> Query. </param>
        /// <param name="sqlSelectQueryParameters"> Parameters. </param>
        /// <returns> Result. </returns>
        List<Dictionary<string, object>> ExecuteSelectQuery(string sqlSelectQuery, List<SqlParameter> sqlSelectQueryParameters = null);
    }
}