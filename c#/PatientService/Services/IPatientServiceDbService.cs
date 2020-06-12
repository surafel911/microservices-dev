using System;

using PatientService.Models;

namespace PatientService.Services
{
	public interface IPatientServiceDbService
	{
		bool CanConnect();
		void EnsureCreated();
		void EnsureDeleted();
		void AddPatient(Patient patient);
		Patient FindPatient(Guid id);
		Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth);
		void UpdatePatient(Patient patient);
		void RemovePatient(Patient patient);
		void AddPatientContact(PatientContact patientContact);
		PatientContact FindPatientContact(Guid patientId);
		void RemovePatientContact(PatientContact patientContact);
		void UpdatePatientContact(PatientContact patientContact);
	}
}
