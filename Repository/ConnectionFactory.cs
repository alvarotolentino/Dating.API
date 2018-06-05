using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace Dating.API.Repository
{
    public class ConnectionFactory : IConnectionFactory
    {
        private SqliteConnectionStringBuilder _connectionStringBuilder;
        private readonly string _connectionString = "DB/dating.db";
        public IDbConnection GetConnection
        {
            get
            {
                SQLitePCL.Batteries.Init();
                _connectionStringBuilder = new SqliteConnectionStringBuilder();
                _connectionStringBuilder.DataSource = _connectionString;
                var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString);
                return connection;
            }
        }
    }
}