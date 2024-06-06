using api.Models;

namespace api.Data.Repositories.Abstraction
{
    public interface IContactRepository : IRepository<Contact>
    {
        int CountByTerm(string term);
    }
}