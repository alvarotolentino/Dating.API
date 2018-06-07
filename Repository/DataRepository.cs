using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dating.API.Models;

namespace Dating.API.Repository
{
    public class DataRepository : GenericRepository<Value>, IDataRepository
    {
        IConnectionFactory _connectionFactory;
        public DataRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public new Task<Value> Get(long Id)
        {
            var sql = string.Empty;
            sql = @"SELECT * FROM [values] WHERE Id = @Id";

            using (var connection = _connectionFactory.GetConnection)
            {
                var result = connection.QueryFirstOrDefaultAsync<Value>(sql, new { @Id = Id });
                return result;
            }
        }

        public new Task<IEnumerable<Value>> GetAll()
        {
            var sql = string.Empty;
            sql = @"SELECT * FROM [values]";
            using (var connection = _connectionFactory.GetConnection)
            {
                var result = connection.QueryAsync<Value>(sql);
                return result;
            }
        }


    }
}