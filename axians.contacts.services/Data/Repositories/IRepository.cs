using System.Collections.Generic;

namespace axians.contacts.services.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        int Add(T item);

        void Remove(int id);

        void Update(T item);

        T FindByID(int id);

        IEnumerable<T> FindAll(string sortField, string term, int pageNumber = 1, int pageSize = 10);
        int CountAll();
    }
}
