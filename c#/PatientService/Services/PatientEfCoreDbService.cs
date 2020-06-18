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
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void EnsureCreated()
		{
			try {
				_patientDbContext.Database.EnsureCreated();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void EnsureDeleted()
		{
			try {
				_patientDbContext.Database.EnsureDeleted();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public bool AnyPatients()
		{
			try {
				return _patientDbContext.Patients.Any();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void AddPatient(Patient patient)
		{
			try {
				_patientDbContext.Patients.Add(patient);
				_patientDbContext.SaveChanges();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void AddPatientRange(IEnumerable<Patient> patients)
		{
			try {
				_patientDbContext.Patients.AddRange(patients);
				_patientDbContext.SaveChanges();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public Patient FindPatient(Guid id)
		{
			try {
				return _patientDbContext.Patients.Find(id);
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
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
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void UpdatePatient(Patient patient)
		{
			try {
				_patientDbContext.Patients.Update(patient);
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void RemovePatient(Patient patient)
		{
			try {
				_patientDbContext.Patients.Remove(patient);
				_patientDbContext.SaveChanges();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void AddPatientContact(PatientContact patientContact)
		{
			try {
				_patientDbContext.PatientContacts.Add(patientContact);
				_patientDbContext.SaveChanges();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void AddPatientContactRange(IEnumerable<PatientContact> patientContacts)
		{
			try {
				_patientDbContext.PatientContacts.AddRange(patientContacts);
				_patientDbContext.SaveChanges();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public PatientContact FindPatientContact(Guid patientId)
		{
			try {
				return _patientDbContext.PatientContacts.Find(patientId);
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void RemovePatientContact(PatientContact patientContact)
		{
			try {
				_patientDbContext.PatientContacts.Remove(patientContact);
				_patientDbContext.SaveChanges();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}

		public void UpdatePatientContact(PatientContact patientContact)
		{
			try {
				_patientDbContext.PatientContacts.Update(patientContact);
				_patientDbContext.SaveChanges();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured in the database service.");
				throw;
			}
		}
    }
}
