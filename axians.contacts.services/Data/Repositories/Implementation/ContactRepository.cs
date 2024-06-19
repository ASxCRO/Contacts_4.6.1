using axians.contacts.services.Data.Repositories.Abstraction;
using axians.contacts.services.Models;
using axians.contacts.services.Services.Abstraction;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace axians.contacts.services.Data.Repositories.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly DbConnectionFactory _connectionFactory;
        private readonly IUserContext _userContext;

        public ContactRepository(DbConnectionFactory connectionFactory, IUserContext userContext)
        {
            _connectionFactory = connectionFactory;
            _userContext = userContext;
        }

        public int Add(Contact item)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.QuerySingle<int>(
                    "spManageContact",
                    new { FirstName = item.FirstName, Email = item.Email, LastName = item.LastName, UserId = (int)_userContext.UserId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public int CountAll()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.ExecuteScalar<int>(
                    "spCountAllContacts",
                    new {UserId = (int)_userContext.UserId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public IEnumerable<Contact> FindAll(string sortField, string term, int pageNumber = 1, int pageSize = 10)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.Query<Contact>(
                    "spFindAllContacts",
                    new { PageNumber = pageNumber, PageSize = pageSize, SortField = sortField, Term = term, UserId = (int)_userContext.UserId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public Contact FindByID(int id)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.QuerySingleOrDefault<Contact>(
                    "spFindContactByID",
                    new { ID = id },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public void Remove(int id)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "spManageContact",
                    new { ID = id, IsDelete = true, UserId = (int)_userContext.UserId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public void Update(Contact item)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "spManageContact",
                    new { ID = item.Id, FirstName = item.FirstName, Email = item.Email, LastName = item.LastName, UserId = (int)_userContext.UserId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public int CountByTerm(string term)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.ExecuteScalar<int>(
                    "spCountContactsByTerm",
                    new { Term = term, UserId = (int)_userContext.UserId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
