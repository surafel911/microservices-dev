using System;
using System.Net.Http;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using PatientService.Data;
using PatientService.Services;

/*
 * TODO: Use this API to seed users https://randomuser.me/api
 */
namespace PatientService.Models
{
    public static class SeedData
    {
		private static readonly int PatientCount = 5000;

		private static async Task<string> GetRandomPatientData(HttpClient httpClient)
		{
            return await httpClient.GetStringAsync(
				"https://randomuser.me/api/?nat=us&results=" + PatientCount.ToString());
		}

        public static void Initialize(IServiceProvider serviceProvider)
        {
			HttpClient httpClient;
			PatientServiceDbContext patientServiceDbContext;
			IPatientServiceDbHandler patientServiceDbHandler;

			httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

            patientServiceDbContext = new PatientServiceDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<PatientServiceDbContext>>());

			patientServiceDbHandler = serviceProvider.CreateScope().ServiceProvider
				.GetRequiredService<IPatientServiceDbHandler>();
            			
            if (patientServiceDbContext.Patients.Any()) {
                return;
            }

            ICollection<Patient> patients = new List<Patient>();
            ICollection<PatientContact> patientContacts = new List<PatientContact>();
            
			JsonElement patientsElement = JsonDocument.Parse(GetRandomPatientData(httpClient).Result)
				.RootElement.GetProperty("results");
				
			foreach (JsonElement patientElement in patientsElement.EnumerateArray()) {
 				JsonElement idElement = patientElement.GetProperty("id");
				JsonElement nameElement = patientElement.GetProperty("name");
				JsonElement emailAddressElement = patientElement.GetProperty("email");
				JsonElement dateOfBirthElement = patientElement.GetProperty("dob");
				JsonElement phoneNumberElement = patientElement.GetProperty("cell");
	
				Patient patient = new Patient 
					{
						Id = Guid.NewGuid(),
						FirstName = nameElement.GetProperty("first").GetString(),
						LastName = nameElement.GetProperty("last").GetString(),
						LastFourOfSSN = idElement.GetProperty("value").GetString().Substring(7),
						DateOfBirth = dateOfBirthElement.GetProperty("date").GetDateTime()
					};
	
				PatientContact patientContact = new PatientContact
					{
						PatientId = patient.Id,
						Patient = patient,
						PhoneNumber = phoneNumberElement.GetString(),
						EmailAddress = emailAddressElement.GetString()
					};
			
				patients.Add(patient);
				patientContacts.Add(patientContact);
			}

			patientServiceDbContext.Patients.AddRange(patients);
			patientServiceDbContext.PatientContacts.AddRange(patientContacts);

            patientServiceDbContext.SaveChanges();
        }
    }
}
