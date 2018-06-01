using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dating.API.Models;
using Microsoft.Data.Sqlite;

namespace Dating.API.Repository
{
    public class DataRepository : BaseRepository, IDataRepository
    {
        private SqliteConnectionStringBuilder _connectionStringBuilder;

        public DataRepository()
        {
            SQLitePCL.Batteries.Init();
            _connectionStringBuilder = new SqliteConnectionStringBuilder();
            _connectionStringBuilder.DataSource = _connectionString;
        }
        public IEnumerable<Value> Get(int? id = null)
        {
            var sql = string.Empty;
            sql = id.HasValue ? @"SELECT * FROM [values] WHERE Id = @Id" : @"SELECT * FROM [values]";

            using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
            {
                var result = connection.Query<Value>(sql, new { Id = id });
                return result;
            }
        }

        public Task<IEnumerable<Value>> GetAsync(int? id = null)
        {
            var sql = string.Empty;
            sql = id.HasValue ? @"SELECT * FROM [values] WHERE Id = @Id" : @"SELECT * FROM [values]";
            using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
            {
                var result = connection.QueryAsync<Value>(sql, new { Id = id });
                return result;
            }
        }
    }
}