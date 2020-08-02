using System.Data;
using System.Text;

using Dapper;
using Microsoft.Extensions.Logging;

using PatientService.Models;

namespace PatientService.Services
{
    public class DefaultDbService : IDefaultDbService
    {
        private readonly ILogger<DefaultDbService> _logger;
        private readonly IDbConnection _dbConnection;

        private CommandDefinition GetCreateServiceDbCommand(string dbName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"SELECT 'CREATE DATABASE ");
            stringBuilder.Append(dbName);
            stringBuilder.Append(@"' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = '");
            stringBuilder.Append(dbName);
            stringBuilder.Append(@"');");
            
            return new CommandDefinition(stringBuilder.ToString());
        }

        private CommandDefinition GetDeleteServiceDbCommand(string dbName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"DROP DATABASE IF EXISTS ");
            stringBuilder.Append(dbName);
            stringBuilder.Append(@";");
            
            return new CommandDefinition(stringBuilder.ToString());
        }
            
        
        public DefaultDbService(ILogger<DefaultDbService> logger,
            IDbConnectionFactory dbConnectionFactory)
        {
            _logger = logger;
            _dbConnection = dbConnectionFactory.CreateDbConnection(DbConnectionName.DefaultDbName);
        }

        public void CreateServiceDb(string dbName)
        {
            string query = _dbConnection.QueryFirst<string>(GetCreateServiceDbCommand(dbName));

            _dbConnection.Execute(query ?? string.Empty);
        }

        public void DeleteServiceDb(string dbName)
        {
            _dbConnection.Execute(GetDeleteServiceDbCommand(dbName));
        }
    }
}