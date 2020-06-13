using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using PatientService.Data;
using PatientService.Models;

using Npgsql;

namespace PatientService.Services
{
    public class PatientEfCoreDbService : IPatientDbService
    {
		private readonly ILogger<PatientEfCoreDbService> _logger;
		private readonly PatientDbContext _patientDbContext;

		public PatientEfCoreDbService(ILogger<PatientEfCoreDbService> logger,
			PatientDbContext patientServiceDbContext)
		{
			_logger = logger;
			_patientDbContext = patientServiceDbContext;
		}

		public bool CanConnect()
		{
			try {
				return _patientDbContext.Database.CanConnect();
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
				_patientDbContext.Database.EnsureCreated();
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
				_patientDbContext.Database.EnsureDeleted();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public bool AnyPatients()
		{
			try {
				return _patientDbContext.Patients.Any();
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
				_patientDbContext.Patients.Add(patient);
				_patientDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public void AddPatientRange(IEnumerable<Patient> patients)
		{
			try {
				_patientDbContext.Patients.AddRange(patients);
				_patientDbContext.SaveChanges();
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
				return _patientDbContext.Patients.Find(id);
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
				IQueryable<Patient> patientQueryable = _patientDbContext
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
				_patientDbContext.Patients.Update(patient);
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
				_patientDbContext.Patients.Remove(patient);
				_patientDbContext.SaveChanges();
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
				_patientDbContext.PatientContacts.Add(patientContact);
				_patientDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbServiceException(e);
			} catch (NpgsqlException e) {
				throw new DbServiceException(e);
			} catch (InvalidOperationException e) {
				throw e;
			}
		}

		public void AddPatientContactRange(IEnumerable<PatientContact> patientContacts)
		{
			try {
				_patientDbContext.PatientContacts.AddRange(patientContacts);
				_patientDbContext.SaveChanges();
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
				return _patientDbContext.PatientContacts.Find(patientId);
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
				_patientDbContext.PatientContacts.Remove(patientContact);
				_patientDbContext.SaveChanges();
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
				_patientDbContext.PatientContacts.Update(patientContact);
				_patientDbContext.SaveChanges();
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
