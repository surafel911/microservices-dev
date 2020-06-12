using System;
using System.Data.Common;

using Npgsql;

namespace PatientService.Models
{
    public class DbServiceException : DbException
    {
		public DbServiceException(Exception exception) 
			: base("Database handler reports an error: " + exception.Message, exception)
		{
		}

		public DbServiceException(NpgsqlException exception) 
			: base("Npgsql server reports an error: " + exception.Message, exception)
		{
		}

		public DbServiceException(PostgresException exception)
			: base("PostgreSQL backend reports an error: " + exception.Message, exception)
		{
		}
    }
}
