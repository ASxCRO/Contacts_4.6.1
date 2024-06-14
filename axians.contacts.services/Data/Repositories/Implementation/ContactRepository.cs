using axians.contacts.services.Data.Repositories.Abstraction;
using axians.contacts.services.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace axians.contacts.services.Data.Repositories.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public ContactRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Add(Contact item)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.QuerySingle<int>(
                    "spManageContact",
                    new { FirstName = item.FirstName, Email = item.Email, LastName = item.LastName },
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
                    new { PageNumber = pageNumber, PageSize = pageSize, SortField = sortField, Term = term },
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
                    new { ID = id, IsDelete = true },
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
                    new { ID = item.Id, FirstName = item.FirstName, Email = item.Email, LastName = item.LastName },
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
                    new { Term = term },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
