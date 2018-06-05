using System;
using System.Threading.Tasks;
using Dating.API.Models;
using Dapper;

namespace Dating.API.Repository
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {
        IConnectionFactory _connectionFactory;
        public AuthRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public Task<User> Login(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var insert = @"INSERT Users (Name, PasswordHash, PasswordSalt) VALUES(@name, @passwordHash, @passwordSalt)";
            int idInserted;
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Execute(insert, new { @name = user.UserName, @passwordHash = user.PasswordHash, @passwordSalt = user.PasswordSalt });
                idInserted = (int)connection.ExecuteScalar("select last_insert_rowid()");
                var result = Get(idInserted);
                return await result;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public Task<bool> UserExists(string username)
        {
            throw new System.NotImplementedException();
        }

        public new Task<User> Get(int Id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id";
            using (var connection = _connectionFactory.GetConnection)
            {
                var result = connection.QuerySingleAsync<User>(sql, new { @Id = Id });
                return result;
            }
        }

    }
}