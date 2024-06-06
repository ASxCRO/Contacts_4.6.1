using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Services.Abstraction
{
    public interface IContactService
    {
        int AddContact(Contact item);
        int CountAllContacts();
        IEnumerable<Contact> FindAllContacts(int pageNumber, int pageSize, string sortField, string term);
        Contact FindContactByID(int id);
        void RemoveContact(int id);
        void UpdateContact(Contact item);
        int CountContactsByTerm(string term);
    }
}