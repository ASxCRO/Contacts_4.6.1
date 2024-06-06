using api.Data.Repositories.Abstraction;
using api.Models;
using api.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public int AddContact(Contact item)
        {
            return _contactRepository.Add(item);
        }

        public int CountAllContacts()
        {
            return _contactRepository.CountAll();
        }

        public IEnumerable<Contact> FindAllContacts(int pageNumber, int pageSize, string sortField, string term)
        {
            return _contactRepository.FindAll(pageNumber, pageSize, sortField, term);
        }

        public Contact FindContactByID(int id)
        {
            return _contactRepository.FindByID(id);
        }

        public void RemoveContact(int id)
        {
            _contactRepository.Remove(id);
        }

        public void UpdateContact(Contact item)
        {
            _contactRepository.Update(item);
        }

        public int CountContactsByTerm(string term)
        {
            return _contactRepository.CountByTerm(term);
        }
    }
}