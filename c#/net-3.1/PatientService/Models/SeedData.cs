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
using DataAtThePointOfCare.Models;

namespace PatientService.Models
{
    public static class SeedData
    {
		private static readonly int PatientCount = 100;

		private static async Task<string> GetRandomPatientData(HttpClient httpClient)
		{
            return await httpClient.GetStringAsync(
				"https://randomuser.me/api/?nat=us&results=" + PatientCount.ToString());
		}

		private static Patient GetPatientFromJsonElement(JsonElement patientElement)
		{
			JsonElement idElement = patientElement.GetProperty("id");
			JsonElement nameElement = patientElement.GetProperty("name");
			JsonElement dateOfBirthElement = patientElement.GetProperty("dob");

			return new Patient
				{
					Id = Guid.NewGuid(),
					FirstName = nameElement.GetProperty("first").GetString(),
					LastName = nameElement.GetProperty("last").GetString(),
					LastFourOfSSN = idElement.GetProperty("value").GetString().Substring(7),
					DateOfBirth = dateOfBirthElement.GetProperty("date").GetDateTime()
				};
		}

		private static PatientContact GetPatientContactFromJsonElement(
			JsonElement patientElement,
			Patient patient)
		{
			JsonElement emailAddressElement = patientElement.GetProperty("email");
			JsonElement phoneNumberElement = patientElement.GetProperty("cell");

			return new PatientContact
				{
					PatientId = patient.Id,
					Patient = patient,
					PhoneNumber = phoneNumberElement.GetString(),
					EmailAddress = emailAddressElement.GetString()
				};
		}

        public static void Initialize(
			IPatientDbService patientDbService,
			IHttpClientFactory httpClientFactory)
        {
			HttpClient httpClient;

			httpClient = httpClientFactory.CreateClient();

            ICollection<Patient> patients = new List<Patient>();
            ICollection<PatientContact> patientContacts = new List<PatientContact>();

			JsonElement patientsElement = JsonDocument
				.Parse(GetRandomPatientData(httpClient).Result)
				.RootElement.GetProperty("results");

			foreach (JsonElement patientElement in patientsElement.EnumerateArray()) {
				Patient patient = GetPatientFromJsonElement(patientElement);

				PatientContact patientContact = GetPatientContactFromJsonElement(patientElement, patient);

				patients.Add(patient);
				patientContacts.Add(patientContact);
			}

			patientDbService.AddPatientRange(patients.Cast<Patient>());
			patientDbService.AddPatientContactRange(patientContacts.Cast<PatientContact>());
        }
    }
}
