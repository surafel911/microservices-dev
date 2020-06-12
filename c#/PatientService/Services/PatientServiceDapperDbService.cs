using System;
using System.Linq;

using PatientService.Data;
using PatientService.Models;

namespace PatientService.Services
{
    public class PatientServiceDapperDbService : IPatientServiceDbService
    {
		private readonly PatientServiceDbContext _patientServiceDbContext;

		public PatientServiceDapperDbService(PatientServiceDbContext patientServiceDbContext)
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
