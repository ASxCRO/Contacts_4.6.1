using axians.contacts.services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace axians.contacts.services.Services.Abstraction
{
    public interface IContactService
    {
        int AddContact(Contact item);
        int CountAllContacts();
        ContactGridViewModel FindAllContacts(GetAllContactsRequest model);
        Contact FindContactByID(int id);
        void RemoveContact(int id);
        void UpdateContact(Contact item);
        int CountContactsByTerm(string term);
    }
}