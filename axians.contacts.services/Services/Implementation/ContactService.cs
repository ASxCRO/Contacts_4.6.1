using axians.contacts.services.Services.Abstraction;
using axians.contacts.services.Data.Repositories.Abstraction;
using axians.contacts.services.Models;
using System;

namespace axians.contacts.services.Services.Implementation
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

        public ContactGridViewModel FindAllContacts(GetAllContactsRequest model)
        {
            int totalCount;

            if (model.PageSize < 1) model.PageSize = 10;

            if (string.IsNullOrWhiteSpace(model.Term)) totalCount = CountAllContacts();
            else totalCount = this.CountContactsByTerm(model.Term);

            int totalPages = (int)Math.Ceiling((double)totalCount / model.PageSize);

            if (model.PageNumber > totalPages || model.PageNumber < 1) model.PageNumber = 1;

            var contactGridViewModel = new ContactGridViewModel
            {
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
                TotalPages = totalPages,
                Sort = model.SortField,
                SearchTerm = model.Term,
                TotalItemsNumber = totalCount,
                Contacts = _contactRepository.FindAll(model.SortField, model.Term, model.PageNumber, model.PageSize)
            };

            return contactGridViewModel;
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