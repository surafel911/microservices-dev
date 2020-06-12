using System;
using System.Linq;

using Microsoft.Extensions.Logging;

using PatientService.Data;
using PatientService.Models;

using Npgsql;

namespace PatientService.Services
{
    public class PatientServiceEfCoreDbService : IPatientServiceDbService
    {
		private readonly ILogger<PatientServiceEfCoreDbService> _logger;
		private readonly PatientServiceDbContext _patientServiceDbContext;

		public PatientServiceEfCoreDbService(ILogger<PatientServiceEfCoreDbService> logger,
			PatientServiceDbContext patientServiceDbContext)
		{
			_logger = logger;
			_patientServiceDbContext = patientServiceDbContext;
		}

		public bool CanConnect()
		{
			try {
				return _patientServiceDbContext.Database.CanConnect();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public void EnsureCreated()
		{
			try {
				_patientServiceDbContext.Database.EnsureCreated();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public void EnsureDeleted()
		{
			try {
				_patientServiceDbContext.Database.EnsureDeleted();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public void AddPatient(Patient patient)
		{
			try {
				_patientServiceDbContext.Patients.Add(patient);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public Patient FindPatient(Guid id)
		{
			try {
				return _patientServiceDbContext.Patients.Find(id);
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth)
		{
			try {
				IQueryable<Patient> patientQueryable = _patientServiceDbContext
					.Patients.AsQueryable().Where(patient =>
						patient.FirstName == firstName && 
						patient.LastName == lastName &&
						patient.DateOfBirth == dateOfBirth);

				if (patientQueryable.Count() == 0) {
					return null;
				} 
				
				return patientQueryable.First();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public void UpdatePatient(Patient patient)
		{
			try {
				_patientServiceDbContext.Patients.Update(patient);
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}  
		}

		public void RemovePatient(Patient patient)
		{
			try {
				_patientServiceDbContext.Patients.Remove(patient);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public void AddPatientContact(PatientContact patientContact)
		{
			try {
				_patientServiceDbContext.PatientContacts.Add(patientContact);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public PatientContact FindPatientContact(Guid patientId)
		{
			try {
				return _patientServiceDbContext.PatientContacts.Find(patientId);
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public void RemovePatientContact(PatientContact patientContact)
		{
			try {
				_patientServiceDbContext.PatientContacts.Remove(patientContact);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public void UpdatePatientContact(PatientContact patientContact)
		{
			try {
				_patientServiceDbContext.PatientContacts.Update(patientContact);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}
    }
}
