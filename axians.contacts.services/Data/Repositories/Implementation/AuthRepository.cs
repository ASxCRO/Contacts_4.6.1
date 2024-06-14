using System.Data;
using System.Reflection;
using axians.contacts.services.Data.Repositories.Abstraction;
using Dapper;

namespace axians.contacts.services.Data.Repositories.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public AuthRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool UserExists(string username)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username";
                var count = db.ExecuteScalar<int>(query, new { Username = username });
                return count > 0;
            }
        }

        public int RegisterUser(string username, string password, string fullName)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                string query = @"INSERT INTO Users (Username, PasswordHash, FullName)
                                 VALUES (@Username, @PasswordHash, @FullName);
                                 SELECT CAST(SCOPE_IDENTITY() as int)";
                var parameters = new { Username = username, PasswordHash = password, FullName = fullName };
                return db.QueryFirstOrDefault<int>(query, parameters);
            }
        }

        public bool ValidateUser(string username, string password)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";
                var storedPassword =  db.ExecuteScalar<string>(query, new { Username = username });

                return storedPassword == password;
            }
        }
    }
}
