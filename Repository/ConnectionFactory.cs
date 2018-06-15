using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Dating.API.Repository {
    public class ConnectionFactory : IConnectionFactory {
        private SqliteConnectionStringBuilder _connectionStringBuilder;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ConnectionFactory (IConfiguration configuration) {
            _configuration = configuration;
            _connectionString = _configuration.GetSection ("ConnectionStrings:DefaultConnection").Value;
        }
        public IDbConnection GetConnection {
            get {
                SQLitePCL.Batteries.Init ();
                _connectionStringBuilder = new SqliteConnectionStringBuilder ();
                _connectionStringBuilder.DataSource = _connectionString;
                var connection = new SqliteConnection (_connectionStringBuilder.ConnectionString);
                return connection;
            }
        }

    }
}