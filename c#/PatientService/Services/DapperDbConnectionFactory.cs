using System;
using System.Data;
using System.Collections.Generic;

using Npgsql;

using PatientService.Models;

namespace PatientService.Services
{
    public class DapperDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IDictionary<DbConnectionName, string> _connectionDictionary;

        public DapperDbConnectionFactory(IDictionary<DbConnectionName, string> connectionDictionary)
        {
            _connectionDictionary = connectionDictionary;
        }
        
        public IDbConnection CreateDbConnection(DbConnectionName connectionName)
        {
            if (_connectionDictionary.TryGetValue(connectionName, out string connectionString))
            {
                return new NpgsqlConnection(connectionString);
            }

            throw new ArgumentNullException();
        }
    }
}