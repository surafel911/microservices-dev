using System.Data;

using DataAtThePointOfCare.Models;

namespace DataAtThePointOfCare.Services
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection(DbConnectionName connectionName);
    }
}