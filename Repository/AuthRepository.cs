using System;
using System.Threading.Tasks;
using Dating.API.Models;
using Dapper;
using System.Linq;

namespace Dating.API.Repository
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {
        IConnectionFactory _connectionFactory;
        public AuthRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> Login(string username, string password)
        {
            var sql = "SELECT * FROM Users WHERE UserName = @username";
            User user = null;
            using (var connection = _connectionFactory.GetConnection)
            {
                user = await connection.QuerySingleAsync<User>(sql, new { @username = username });
            }
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;

        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var insert = @"INSERT INTO Users (UserName, PasswordHash, PasswordSalt) VALUES(@name, @passwordHash, @passwordSalt); SELECT last_insert_rowid()";

            long idInserted;
            using (var connection = _connectionFactory.GetConnection)
            {
                idInserted = await connection.ExecuteScalarAsync<long>(insert,
                new
                {
                    @name = user.UserName,
                    @passwordHash = user.PasswordHash,
                    @passwordSalt = user.PasswordSalt
                });
                var result = await Get(idInserted);
                return result;
            }
        }

        public async Task<bool> UserExists(string username)
        {
            var sql = "SELECT * FROM Users WHERE UserName = @username";
            using (var connection = _connectionFactory.GetConnection)
            {
                var result = await connection.QueryAsync<User>(sql, new { @username = username });
                if (result != null & result.Any())
                {
                    return true;
                }
                return false;
            }
        }

        public new Task<User> Get(long Id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id";
            using (var connection = _connectionFactory.GetConnection)
            {
                var result = connection.QuerySingleAsync<User>(sql, new { @Id = Id });
                return result;
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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}