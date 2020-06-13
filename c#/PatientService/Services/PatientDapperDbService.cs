using System;
using System.Linq;
using System.Collections.Generic;

using PatientService.Data;
using PatientService.Models;

namespace PatientService.Services
{
    public class PatientDapperDbService : IPatientDbService
    {
		private readonly PatientDbContext _patientDbContext;

		public PatientDapperDbService(PatientDbContext patientDbContext)
		{
			_patientDbContext = patientDbContext;
		}

		public bool CanConnect()
		{
			return false;
		}

		public void EnsureCreated()
		{
		}

		public void EnsureDeleted()
		{
		}

		public bool AnyPatients()
		{
			return false;
		}

		public void AddPatient(Patient patient)
		{
		}

		public void AddPatientRange(IEnumerable<Patient> patients)
		{
		}

		public Patient FindPatient(Guid id)
		{
			return null;
		}

		public Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth)
		{
			return null;
		}

		public void UpdatePatient(Patient patient)
		{
		}

		public void RemovePatient(Patient patient)
		{
		}

		public void AddPatientContact(PatientContact patientContact)
		{
		}

		public void AddPatientContactRange(IEnumerable<PatientContact> patientContacts)
		{
		
		}

		public PatientContact FindPatientContact(Guid patientId)
		{
			return null;
		}

		public void RemovePatientContact(PatientContact patientContact)
		{
		}

		public void UpdatePatientContact(PatientContact patientContact)
		{
		}
    }
}
