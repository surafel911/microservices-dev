using System;
using System.Linq;

using PatientService.Data;
using PatientService.Models;

using Npgsql;

namespace PatientService.Services
{
    public class PatientServiceDbHandler : IPatientServiceDbHandler
    {
		private readonly PatientServiceDbContext _patientServiceDbContext;

		public PatientServiceDbHandler(PatientServiceDbContext patientServiceDbContext)
		{
			_patientServiceDbContext = patientServiceDbContext;
		}

		public bool CanConnect()
		{
			try {
				return _patientServiceDbContext.Database.CanConnect();
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			}
		}

		public void EnsureCreated()
		{
			try {
				_patientServiceDbContext.Database.EnsureCreated();
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			}
		}

		public void EnsureDeleted()
		{
			try {
				_patientServiceDbContext.Database.EnsureDeleted();
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			}
		}

		public void AddPatient(Patient patient)
		{
			try {
				_patientServiceDbContext.Patients.Add(patient);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			} 
		}

		public Patient FindPatient(params object[] keyValues)
		{
			try {
				return _patientServiceDbContext.Patients.Find(keyValues);
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			} 
		}

		public Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth)
		{
			try {
				return _patientServiceDbContext.Patients.AsQueryable().Where(patient =>
					patient.FirstName == firstName && patient.LastName == lastName &&
					patient.DateOfBirth == dateOfBirth).First();
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			} 
		}

		public void UpdatePatient(Patient patient)
		{
			try {
				_patientServiceDbContext.Patients.Update(patient);
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			} 
		}

		public void RemovePatient(Patient patient)
		{
			try {
				_patientServiceDbContext.Patients.Remove(patient);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			} 
		}

		public void AddPatientContact(PatientContact patientContact)
		{
			try {
				_patientServiceDbContext.PatientContacts.Add(patientContact);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			} 
		}

		public PatientContact FindPatientContact(params object[] keyValues)
		{
			try {
				return _patientServiceDbContext.PatientContacts.Find(keyValues);
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			} 
		}

		public void RemovePatientContact(PatientContact patientContact)
		{
			try {
				_patientServiceDbContext.PatientContacts.Remove(patientContact);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			} 
		}

		public void UpdatePatientContact(PatientContact patientContact)
		{
			try {
				_patientServiceDbContext.PatientContacts.Update(patientContact);
				_patientServiceDbContext.SaveChanges();
			} catch (PostgresException e) {
				throw new DbHandlerException(e);
			} catch (NpgsqlException e) {
				throw new DbHandlerException(e);
			} 
		}
    }
}