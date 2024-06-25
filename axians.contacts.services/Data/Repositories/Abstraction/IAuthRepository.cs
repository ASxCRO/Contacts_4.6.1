namespace axians.contacts.services.Data.Repositories.Abstraction
{
    public interface IAuthRepository
    {
        bool UserExists(string username);
        int RegisterUser(string username, string password, string fullName);
        bool ValidateUser(string username, string password);
        int GetIdByUsername(string username);
    }
}
