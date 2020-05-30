using PatientService.Models;

namespace PatientService.Services
{
	public interface IPatientServiceDbHandler
	{
		void CanConnect();
		void EnsureCreated();
		void EnsureDeleted();
		void AddPatient(Patient patient);
		void FindPatient(params object[] keyValues);
		void UpdatePatient(Patient patient);
		void RemovePatient(Patient patient);
		void AddPatientContact(PatientContact patientContact);
		void FindPatientContact(params object[] keyValues);
		void RemovePatientContact(PatientContact patientContact);
		void UpdatePatientContact(PatientContact patientContact);
	}
}