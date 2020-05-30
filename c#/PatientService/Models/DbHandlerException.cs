using System.Data.Common;

using Npgsql;

namespace PatientService.Models
{
    public class DbHandlerException : DbException
    {
		public DbHandlerException(NpgsqlException exception) 
			: base("Npgsql server reports an error: " + exception.Message, exception)
		{
		}

		public DbHandlerException(PostgresException exception)
			: base("PostgreSQL backend reports an error: " + exception.Message, exception)
		{
		}
    }
}