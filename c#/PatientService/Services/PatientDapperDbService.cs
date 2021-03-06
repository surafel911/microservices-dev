using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Dapper;
using Microsoft.Extensions.Logging;

using PatientService.Models;
using DataAtThePointOfCare.Models;
using DataAtThePointOfCare.Services;

namespace PatientService.Services
{
	public class PatientDapperDbService : IPatientDbService
	{
		private readonly ILogger<PatientDapperDbService> _logger;
		private readonly IDbConnection _dbConnection;
		private readonly IDefaultDbService _defaultDbService;
		private readonly IPatientDbCommandService _patientDbCommandService;
		
		public PatientDapperDbService(ILogger<PatientDapperDbService> logger,
			IDbConnectionFactory dbConnectionFactory,
			IDefaultDbService defaultDbService,
			IPatientDbCommandService patientDbCommandService)
		{
			_logger = logger;
			_dbConnection = dbConnectionFactory.CreateDbConnection(DbConnectionName.PatientDbName);
			_defaultDbService = defaultDbService;
			_patientDbCommandService = patientDbCommandService;
		}

		public bool CanConnect()
		{
			try {
				_dbConnection.Execute(_patientDbCommandService.GetCanConnectCommand());
			}
			catch (Exception e) {
				_logger.LogError(e, "An error occured in the database service.");
				throw;
			}

			return true;
		}

		public void EnsureCreated()
		{
			try {
				_defaultDbService.CreateServiceDb(_dbConnection.Database);
				_dbConnection.Execute(_patientDbCommandService.GetUUIDModuleCommand());
				_dbConnection.Execute(_patientDbCommandService.GetCreatePatientCommand());
				_dbConnection.Execute(_patientDbCommandService.GetCreatePatientContactCommand());
			}  catch (Exception e) {
				_logger.LogError(e, "An error occured in the database service.");
				throw;
			} 
		}

		public void EnsureDeleted()
		{
			try {
				_defaultDbService.DeleteServiceDb(_dbConnection.Database);
			}  catch (Exception e) {
				_logger.LogError(e, "An error occured in the database service.");
				throw;
			}
		}

		public bool AnyPatients()
		{
			try {
				return _dbConnection.Query<Patient>(_patientDbCommandService.GetAnyPatientCommand()).Any();
			}  catch (Exception e) {
				_logger.LogError(e, "An error occured in the database service.");
				throw;
			}
		}

		public void AddPatient(Patient patient)
		{
			try {
				patient.Id = Guid.NewGuid();
				_dbConnection.Execute(_patientDbCommandService.GetAddPatientCommand(patient));
			}  catch (Exception e) {
				_logger.LogError(e, "An error occured in the database service.");
				throw;
			}
		}

		public void AddPatientRange(IEnumerable<Patient> patients)
		{
			try {
				List<Patient> patientList = patients.ToList();
				
				foreach (Patient patient in patientList) {
					patient.Id = Guid.NewGuid();
				}
				
				_dbConnection.Execute(_patientDbCommandService.GetAddPatientRangeCommand(patientList));
			}  catch (Exception e) {
				_logger.LogError(e, "An error occured in the database service.");
				throw;
			}
		}

		public Patient FindPatient(Guid id)
		{
			try {
				return _dbConnection.QueryFirst<Patient>(_patientDbCommandService.GetFindPatientCommand(id));
			}  catch (Exception e) {
				_logger.LogError(e, "An error occured in the database service.");
				throw;
			}
		}

		public Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth)
		{
			try {
				return _dbConnection.QueryFirst<Patient>(_patientDbCommandService.GetFindPatientCommand(firstName, lastName, dateOfBirth));
			}  catch (Exception e) {
				_logger.LogError(e, "An error occured in the database service.");
				throw;
			}
		}

		public void UpdatePatient(Patient patient)
		{
			throw new NotImplementedException();
		}

		public void RemovePatient(Patient patient)
		{
			throw new NotImplementedException();
		}

		public void AddPatientContact(PatientContact patientContact)
		{
			throw new NotImplementedException();
		}

		public void AddPatientContactRange(IEnumerable<PatientContact> patientContacts)
		{
			throw new NotImplementedException();
		}

		public PatientContact FindPatientContact(Guid patientId)
		{
			throw new NotImplementedException();
		}

		public void UpdatePatientContact(PatientContact patientContact)
		{
			throw new NotImplementedException();
		}

		public void RemovePatientContact(PatientContact patientContact)
		{
			throw new NotImplementedException();
		}
    }
}
