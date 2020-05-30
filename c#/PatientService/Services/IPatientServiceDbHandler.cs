using System;

using PatientService.Models;

namespace PatientService.Services
{
	public interface IPatientServiceDbHandler
	{
		bool CanConnect();
		void EnsureCreated();
		void EnsureDeleted();
		void AddPatient(Patient patient);
		Patient FindPatient(params object[] keyValues);
		Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth);
		void UpdatePatient(Patient patient);
		void RemovePatient(Patient patient);
		void AddPatientContact(PatientContact patientContact);
		PatientContact FindPatientContact(params object[] keyValues);
		void RemovePatientContact(PatientContact patientContact);
		void UpdatePatientContact(PatientContact patientContact);
	}
}