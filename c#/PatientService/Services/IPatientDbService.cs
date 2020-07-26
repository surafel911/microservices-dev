using System;
using System.Collections.Generic;

using PatientService.Models;

namespace PatientService.Services
{
	public interface IPatientDbService
	{
		bool CanConnect();
		void EnsureCreated();
		void EnsureDeleted();
		bool AnyPatients();
		void AddPatient(Patient patient);
		void AddPatientRange(IEnumerable<Patient> patients);
		Patient FindPatient(Guid id);
		Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth);
		void UpdatePatient(Patient patient);
		void RemovePatient(Patient patient);
		void AddPatientContact(PatientContact patientContact);
		void AddPatientContactRange(IEnumerable<PatientContact> patientContacts);
		PatientContact FindPatientContact(Guid patientId);
		void UpdatePatientContact(PatientContact patientContact);
		void RemovePatientContact(PatientContact patientContact);
	}
}
