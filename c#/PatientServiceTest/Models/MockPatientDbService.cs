using System;
using System.Linq;
using System.Collections.Generic;

using PatientService.Models;
using PatientService.Services;
using DataAtThePointOfCare.Models;
using DataAtThePointOfCare.Services;

namespace PatientServiceTest.Models
{
	public class MockPatientDbService : IPatientDbService
	{
		private List<Patient> _patientList;
		private List<PatientContact> _patientContactList;

		public List<Patient> PatientList
		{
			get => _patientList;
		}

		public List<PatientContact> PatientContactList
		{
			get => _patientContactList;
		}

		public MockPatientDbService()
		{
			_patientList = new List<Patient>();
			_patientContactList = new List<PatientContact>();
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
			_patientList.Add(patient);
		}

		public void AddPatientRange(IEnumerable<Patient> patients)
		{
			_patientList.AddRange(patients);
		}

		public Patient FindPatient(Guid id)
		{
			try {
				return _patientList.Find(patient => patient.Id == id);
			} catch {
				return null;
			}
		}

		public Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth)
		{
			try {
				return _patientList.Find(patient =>
					patient.FirstName == firstName &&
					patient.LastName == lastName &&
					patient.DateOfBirth == dateOfBirth);
			} catch {
				return null;
			}
		}

		public void UpdatePatient(Patient patient)
		{
			int index = _patientList.FindIndex(p => p.Id == patient.Id);

			if (index == -1) {
				throw new InvalidOperationException();
			}

			_patientList[index].FirstName = patient.FirstName;
			_patientList[index].LastName = patient.LastName;
			_patientList[index].LastFourOfSSN = patient.LastFourOfSSN;
		}

		public void RemovePatient(Patient patient)
		{
			_patientList.Remove(patient);
		}

		public void AddPatientContact(PatientContact patientContact)
		{
			_patientContactList.Add(patientContact);
		}

		public void AddPatientContactRange(IEnumerable<PatientContact> patientContacts)
		{
			_patientContactList.AddRange(patientContacts);
		}

		public PatientContact FindPatientContact(Guid patientId)
		{
			try {
				return _patientContactList.Find(patient => patient.PatientId == patientId);
			} catch {
				return null;
			}
		}

		public void UpdatePatientContact(PatientContact patientContact)
		{
			int index = _patientContactList.FindIndex(pc => pc.Patient == patientContact.Patient &&
				pc.PatientId == patientContact.PatientId);

			if (index == -1) {
				throw new InvalidOperationException();
			}

			_patientContactList[index].PhoneNumber = patientContact.PhoneNumber;
			_patientContactList[index].EmailAddress = patientContact.EmailAddress;
		}

		public void RemovePatientContact(PatientContact patientContact)
		{
			_patientContactList.Remove(patientContact);
		}
	}
}
