using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Dating.API.Models;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;

namespace Dating.API.Repository
{
    public class DataRepository : BaseRepository, IDataRepository
    {
        private SqliteConnectionStringBuilder _connectionStringBuilder;

        public DataRepository()
        {
            _connectionStringBuilder = new SqliteConnectionStringBuilder();
            _connectionStringBuilder.DataSource = _connectionString;
        }

        public Value RetriveById(int id)
        {
            var result = Get(id);
            if (result != null)
            {
                return result.First();
            }
            return null;
        }

        public IEnumerable<Value> Get(int? id = null)
        {
            var sql = @"select * 
            from values 
            where id = null or id = @id";

            using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
            {
                var result = connection.Query<Value>(sql, new { @id = id });
                return result;
            }
        }
    }
}