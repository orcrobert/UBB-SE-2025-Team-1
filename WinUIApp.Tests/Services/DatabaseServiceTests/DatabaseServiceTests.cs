using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using WinUIApp.Services;
using Xunit;

namespace WinUIApp.Tests.Services.DatabaseServiceTests
{
    public class DatabaseServiceTests
    {
        private readonly DatabaseService _service;

        public DatabaseServiceTests()
        {
            var connectionField = typeof(DatabaseService)
                .GetField("_databaseServiceInstance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            connectionField?.SetValue(null, null);

            _service = DatabaseService.Instance;
        }

        [Fact]
        public void ExecuteSelectQuery_InvalidSql_ReturnsEmptyList()
        {
            var result = _service.ExecuteSelectQuery("SELECT * FROM NonExistentTable");

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ExecuteSelectQuery_WithInvalidParameterCount_ReturnsEmpty()
        {
            var sql = "SELECT * FROM TempTable WHERE Id = @id"; // But no parameters passed

            var result = _service.ExecuteSelectQuery(sql);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ExecuteDataModificationQuery_InvalidSyntax_ReturnsZero()
        {
            var result = _service.ExecuteDataModificationQuery("INSER INTO TempTable VALUES (1)");

            Assert.Equal(0, result);
        }

        [Fact]
        public void ExecuteDataModificationQuery_MissingParameter_ReturnsZero()
        {
            var sql = "INSERT INTO TempTable (Id) VALUES (@id)";
            var result = _service.ExecuteDataModificationQuery(sql); // No parameters

            Assert.Equal(0, result);
        }




    }
}
