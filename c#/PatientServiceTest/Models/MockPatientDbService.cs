using System;
using System.Linq;
using System.Collections.Generic;

using PatientService.Models;
using PatientService.Services;

namespace PatientServiceTest.Models
{
	public class MockPatientDbService : IPatientDbService
	{
		private IList<Patient> patientList;
		private IList<PatientContact> patientContactList;

		public MockPatientDbService()
		{
			patientList = new List<Patient>();
			patientContactList = new List<PatientContact>();
		}

		public bool CanConnect()
		{
			return true;
		}

		public void EnsureCreated()
		{
		}

		public void EnsureDeleted()
		{
		}

		public bool AnyPatients()
		{
			return true;
		}

		public void AddPatient(Patient patient)
		{
			patientList.Add(patient);
		}

		public void AddPatientRange(IEnumerable<Patient> patients)
		{
		}

		public Patient FindPatient(Guid id)
		{
			try {
				return patientList.AsEnumerable().Where(patient => patient.Id == id).First();
			} catch {
				return null;
			}
		}

		public Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth)
		{
			try {
				return patientList.AsEnumerable().Where(patient =>
					patient.FirstName == firstName &&
					patient.LastName == lastName &&
					patient.DateOfBirth == dateOfBirth).First();
			} catch {
				return null;
			}
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

		public PatientContact FindPatientContact(Guid parentId)
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
