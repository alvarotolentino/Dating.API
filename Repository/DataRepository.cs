using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Dating.API.Models;
using Microsoft.Data.Sqlite;

namespace Dating.API.Repository
{
    public class DataRepository : BaseRepository, IDataRepository
    {
        private IDbConnection _connection;

        public DataRepository()
        :base()
        {
            _connection = new SqliteConnection(_connectionString);
        }

        private IDbConnection GetConnection(bool open = true)
        {
            // if (open)
            // {
            //     _connection.Open();
            // }
            return _connection;
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
            using (var cnx = GetConnection())
            {
                var result = cnx.Query<Value>(sql, new{@id = id});
                return result;
            }
        }
    }
}