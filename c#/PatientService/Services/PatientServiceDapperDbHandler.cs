using System;
using System.Linq;

using PatientService.Data;
using PatientService.Models;

namespace PatientService.Services
{
    public class PatientServiceDapperDbHandler : IPatientServiceDbHandler
    {
		private readonly PatientServiceDbContext _patientServiceDbContext;

		public PatientServiceDapperDbHandler(PatientServiceDbContext patientServiceDbContext)
		{
			_patientServiceDbContext = patientServiceDbContext;
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

		public void AddPatient(Patient patient)
		{
		}

		public Patient FindPatient(params object[] keyValues)
		{
			return new Patient();
		}

		public Patient FindPatient(string firstName, string lastName, DateTime dateOfBirth)
		{
			return new Patient();
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

		public PatientContact FindPatientContact(params object[] keyValues)
		{
			return new PatientContact();
		}

		public void RemovePatientContact(PatientContact patientContact)
		{
		}

		public void UpdatePatientContact(PatientContact patientContact)
		{
		}
    }
}
