using System;

using PatientService.Models;
using PatientService.Services;
using DataAtThePointOfCare.Models;
using DataAtThePointOfCare.Services;

namespace PatientServiceTest.Models
{
    public static class TestUtilities
    {
        public static readonly Patient Patient = new Patient()
        {
            Id = Guid.NewGuid(),
            FirstName = "Mason",
            LastName = "Johnson",
            DateOfBirth = DateTime.Now - TimeSpan.FromDays(7),
            LastFourOfSSN = "1234",
        };

        public static readonly PatientContact PatientContact = new PatientContact()
        {
            PatientId = Patient.Id,
            Patient = Patient,
            PhoneNumber = "111-111-1111",
            EmailAddress = "masonjohnson@gmail.com",
        };
        
        public static void InitializeDb(IPatientDbService patientDbService)
        {
            patientDbService.AddPatient(Patient);
            patientDbService.AddPatientContact(PatientContact);
        }
    }
}