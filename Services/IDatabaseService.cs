using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace WinUIApp.Services
{
    public interface IDatabaseService
    {
        static abstract DatabaseService Instance { get; }

        int ExecuteDataModificationQuery(string sqlDataModificationQuery, List<SqlParameter> sqlDataModificationQueryParameters = null);
        List<Dictionary<string, object>> ExecuteSelectQuery(string sqlSelectQuery, List<SqlParameter> sqlSelectQueryParameters = null);
    }
}