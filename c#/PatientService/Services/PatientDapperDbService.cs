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
			try {
				
			} catch (Exception e) {
				
			}
			
			throw new NotImplementedException();
		}

		public void EnsureCreated()
		{
			throw new NotImplementedException();
		}

		public void EnsureDeleted()
		{
			throw new NotImplementedException();
		}

		public bool AnyPatients()
		{
			throw new NotImplementedException();
		}

		public void AddPatient(Patient patient)
		{
			throw new NotImplementedException();
		}

		public void AddPatientRange(IEnumerable<Patient> patients)
		{
			throw new NotImplementedException();
		}

		public Patient FindPatient(Guid id)
		{
			throw new NotImplementedException();
		}

		public Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth)
		{
			throw new NotImplementedException();
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
