using axians.contacts.services.Models;

namespace axians.contacts.services.Data.Repositories.Abstraction
{
    public interface IContactRepository : IRepository<Contact>
    {
        int CountByTerm(string term);
    }
}