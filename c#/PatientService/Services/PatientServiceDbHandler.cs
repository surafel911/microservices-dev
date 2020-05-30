using PatientService.Data;
using PatientService.Models;

namespace PatientService.Services
{
    public class PatientServiceDbHandler : IPatientServiceDbHandler
    {
		private readonly PatientServiceDbContext _patientServiceDbContext;

		public PatientServiceDbHandler(PatientServiceDbContext patientServiceDbContext)
		{
			_patientServiceDbContext = patientServiceDbContext;
		}

		public void CanConnect()
		{
			_patientServiceDbContext.Database.CanConnect();
		}

		public void EnsureCreated()
		{
			_patientServiceDbContext.Database.EnsureCreated();
		}

		public void EnsureDeleted()
		{
			_patientServiceDbContext.Database.EnsureDeleted();
		}

		public void AddPatient(Patient patient)
		{
			_patientServiceDbContext.Patients.Add(patient);
			_patientServiceDbContext.SaveChanges();
		}

		public void FindPatient(params object[] keyValues)
		{
			_patientServiceDbContext.Patients.Find(keyValues);
			_patientServiceDbContext.SaveChanges();
		}

		public void UpdatePatient(Patient patient)
		{
			_patientServiceDbContext.Patients.Update(patient);
			_patientServiceDbContext.SaveChanges();
		}

		public void RemovePatient(Patient patient)
		{
			_patientServiceDbContext.Patients.Remove(patient);
			_patientServiceDbContext.SaveChanges();
		}

		public void AddPatientContact(PatientContact patientContact)
		{
			_patientServiceDbContext.PatientContacts.Add(patientContact);
			_patientServiceDbContext.SaveChanges();
		}

		public void FindPatientContact(params object[] keyValues)
		{
			_patientServiceDbContext.PatientContacts.Find(keyValues);
			_patientServiceDbContext.SaveChanges();
		}

		public void RemovePatientContact(PatientContact patientContact)
		{
			_patientServiceDbContext.PatientContacts.Remove(patientContact);
			_patientServiceDbContext.SaveChanges();
		}

		public void UpdatePatientContact(PatientContact patientContact)
		{
			_patientServiceDbContext.PatientContacts.Update(patientContact);
			_patientServiceDbContext.SaveChanges();
		}
    }
}