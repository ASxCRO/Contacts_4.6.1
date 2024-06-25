using System.Data;
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
                string procedureName = "spUserExists";
                var count = db.ExecuteScalar<int>(
                    procedureName,
                    new { Username = username },
                    commandType: CommandType.StoredProcedure
                );
                return count > 0;
            }
        }

        public int RegisterUser(string username, string password, string fullName)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                string procedureName = "spRegisterUser";
                var parameters = new { Username = username, PasswordHash = password, FullName = fullName };
                return db.QueryFirstOrDefault<int>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public bool ValidateUser(string username, string password)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                string procedureName = "spValidateUser";
                var storedPassword = db.ExecuteScalar<string>(
                    procedureName,
                    new { Username = username },
                    commandType: CommandType.StoredProcedure
                );

                return storedPassword == password;
            }
        }

        public int GetIdByUsername(string username)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                string procedureName = "spGetIdByUsername";
                var id = db.ExecuteScalar<int>(
                    procedureName,
                    new { Username = username },
                    commandType: CommandType.StoredProcedure
                );
                return id;
            }
        }
    }
}
