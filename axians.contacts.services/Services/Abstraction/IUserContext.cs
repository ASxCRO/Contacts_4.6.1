namespace axians.contacts.services.Services.Abstraction
{
    public interface IUserContext
    {
        string UserName { get; }
        int UserId { get; }
    }
}
