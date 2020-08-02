using System.Data;

using PatientService.Models;

namespace PatientService.Services
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection(DbConnectionName connectionName);
    }
}