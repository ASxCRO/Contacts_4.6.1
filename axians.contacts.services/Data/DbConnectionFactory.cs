
namespace axians.contacts.services.Data
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public System.Data.IDbConnection CreateConnection()
        {
            return new System.Data.SqlClient.SqlConnection(_connectionString);
        }
    }
}