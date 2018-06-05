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
        public async Task<User> Login(string username, string password)
        {
            var sql = "SELECT * FROM Users WHERE Name = @username";
            User user = null;
            using (var connection = _connectionFactory.GetConnection)
            {
                user = await connection.QuerySingleAsync<User>(sql, new { @username = username });
            }
            if(user ==null){
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;

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
                var result = await Get(idInserted);
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

        public Task<bool> UserExists(string username)
        {
            
            using (var connection = _connectionFactory.GetConnection)
            {

            }
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