using api.Data.Repositories.Abstraction;
using api.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace api.Data.Repositories.Implementation
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
                    "sp_AddContact",
                    new { item.FirstName, item.Email, item.LastName },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public int CountAll()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.ExecuteScalar<int>(
                    "sp_CountAllContacts",
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public IEnumerable<Contact> FindAll(int pageNumber, int pageSize, string sortField, string term)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.Query<Contact>(
                    "sp_FindAllContacts",
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
                    "sp_FindContactByID",
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
                    "sp_RemoveContact",
                    new { ID = id },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public void Update(Contact item)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "sp_UpdateContact",
                    new { item.Id, item.FirstName, item.Email, item.LastName },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public int CountByTerm(string term)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.ExecuteScalar<int>(
                    "sp_CountContactsByTerm",
                    new { Term = term },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}