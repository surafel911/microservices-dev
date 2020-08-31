using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using PatientService.Models;
using PatientService.Services;
using PatientService.Controllers;
using DataAtThePointOfCare.Models;
using DataAtThePointOfCare.Services;

using PatientService;
using PatientServiceTest.Models;

namespace PatientServiceTest
{
	public class PatientServiceIntegrationTest
		: IClassFixture<CustomWebApplicationFactory<Startup>>
	{
		private readonly HttpClient _httpClient;

		public PatientServiceIntegrationTest(CustomWebApplicationFactory<Startup> factory)
		{
			// Arrange
			_httpClient = factory.CreateClient();
		}

		[Fact]
		public async Task TestGetPatientFromId()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient/" + TestUtilities.Patient.Id);

			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status200OK, (int)httpResponseMessage.StatusCode);
			Assert.Equal("application/json; charset=utf-8", httpResponseMessage.Content.Headers.ContentType.ToString());
			Assert.Equal(JsonSerializer.Serialize(TestUtilities.Patient), httpResponseMessage.Content.ReadAsStringAsync().Result, true);
		}

		[Fact]
		public async Task TestGetPatientEmptyId()
		{
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient/" + Guid.Empty);

			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		[Fact]
		public async Task TestGetPatientInvalidId()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient/" + Guid.NewGuid());

			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status404NotFound, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestGetPatientFromQuery()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient?" +
                "firstName=" + TestUtilities.Patient.FirstName + "&" +
                "lastName=" + TestUtilities.Patient.LastName + "&" +
                "dateOfBirth=" + TestUtilities.Patient.DateOfBirth.ToString("d"));
			
			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status200OK, (int)httpResponseMessage.StatusCode);
			Assert.Equal("application/json; charset=utf-8", httpResponseMessage.Content.Headers.ContentType.ToString());
			Assert.Equal(JsonSerializer.Serialize(TestUtilities.Patient), httpResponseMessage.Content.ReadAsStringAsync().Result, true);
		}

		[Theory]
		[InlineData("")]
		[InlineData("This-string-is-over-fourty-characters.")]
		public async Task TestGetPatientQueryInvalidName(string name)
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient?" +
	            "firstName=" + name + "&" +
	            "lastName=" + name + "&" +
	            "dateOfBirth=" + TestUtilities.Patient.DateOfBirth.ToString("d"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		[Fact]
		public async Task TestGetPatientQueryInvalidDate()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient?" +
                "firstName=" + TestUtilities.Patient.FirstName + "&" +
                "lastName=" + TestUtilities.Patient.LastName + "&" +
                "dateOfBirth=");
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		[Fact]
		public async Task TestCreatePatient()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/", 
				new StringContent(
					JsonSerializer.Serialize(new Patient()
					{
						Id = Guid.NewGuid(),
						FirstName = "John",
						LastName = "Smith",
						LastFourOfSSN = "1234",
						DateOfBirth = DateTime.Now.AddDays(-7),
					}), Encoding.UTF8, "application/json"));

			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status204NoContent, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestCreatePatientEmptyId()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/", 
				new StringContent(
					JsonSerializer.Serialize(new Patient()
					{
						Id = Guid.Empty,
						FirstName = "John",
						LastName = "Smith",
						LastFourOfSSN = "1234",
						DateOfBirth = DateTime.Now.AddDays(-7),
					}), Encoding.UTF8, "application/json"));

			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		[Theory]
		[InlineData("")]
		[InlineData("This string is over fourty characters.")]
		public async Task TestCreatePatientInvalidName(string name)
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/", 
				new StringContent(
					JsonSerializer.Serialize(new Patient()
					{
						Id = Guid.NewGuid(),
						FirstName = name,
						LastName = name,
						LastFourOfSSN = "1234",
						DateOfBirth = DateTime.Now.AddDays(-7),
					}), Encoding.UTF8, "application/json"));

			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		[Theory]
		[InlineData("")]
		[InlineData("12345")]
		public async Task TestCreatePatientInvalidSSN(string ssn)
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/", 
				new StringContent(
					JsonSerializer.Serialize(new Patient()
					{
						Id = Guid.Empty,
						FirstName = "John",
						LastName = "Smith",
						LastFourOfSSN = ssn,
						DateOfBirth = DateTime.Now.AddDays(-7),
					}), Encoding.UTF8, "application/json"));

			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestCreatePatientInvalidType()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/",
				new StringContent(JsonSerializer.Serialize(25)));
			
			// Assert 
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status415UnsupportedMediaType, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestUpdatePatient()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PatchAsync("/api/patient/" + TestUtilities.Patient.Id,
				new StringContent(JsonSerializer.Serialize(new PatientDTO()
				{
					FirstName = "John",
					LastName = "Smith",
					LastFourOfSSN = TestUtilities.Patient.LastFourOfSSN,
					DateOfBirth = TestUtilities.Patient.DateOfBirth,
				}), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status204NoContent, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestUpdatePatientEmptyId()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PatchAsync("/api/patient/" + Guid.Empty,
				new StringContent(JsonSerializer.Serialize(new PatientDTO()
				{
					FirstName = "John",
					LastName = "Smith",
					LastFourOfSSN = TestUtilities.Patient.LastFourOfSSN,
					DateOfBirth = TestUtilities.Patient.DateOfBirth,
				}), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		[Fact]
		public async Task TestUpdatePatientInvalidId()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PatchAsync("/api/patient/" + Guid.NewGuid(),
				new StringContent(JsonSerializer.Serialize(new PatientDTO()
				{
					FirstName = "John",
					LastName = "Smith",
					LastFourOfSSN = TestUtilities.Patient.LastFourOfSSN,
					DateOfBirth = TestUtilities.Patient.DateOfBirth,
				}), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status404NotFound, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestUpdatePatientInvalidType()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PatchAsync("/api/patient/" + Guid.NewGuid(),
				new StringContent(JsonSerializer.Serialize(25), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}

		[Theory]
		[InlineData("")]
		[InlineData("This string is over fourty characters.")]
		public async Task TestUpdatePatientInvalidDTOName(string name)
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PatchAsync("/api/patient/" + TestUtilities.Patient.Id,
				new StringContent(JsonSerializer.Serialize(new PatientDTO()
				{
					FirstName = name,
					LastName = name,
					LastFourOfSSN = TestUtilities.Patient.LastFourOfSSN,
					DateOfBirth = TestUtilities.Patient.DateOfBirth,
				}), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}

		[Theory]
		[InlineData("")]
		[InlineData("12345")]
		public async Task TestUpdatePatientInvalidDTOSSN(string ssn)
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.PatchAsync("/api/patient/" + TestUtilities.Patient.Id,
				new StringContent(JsonSerializer.Serialize(new PatientDTO()
				{
					FirstName = "John",
					LastName = "Smith",
					LastFourOfSSN = ssn,
					DateOfBirth = TestUtilities.Patient.DateOfBirth,
				}), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestDeletePatient()
		{
			//Act
			HttpResponseMessage httpResponseMessage =
				await _httpClient.DeleteAsync("/api/patient/" + TestUtilities.Patient.Id);
			
			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status204NoContent, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestDeletePatientEmptyId()
		{
			//Act
			HttpResponseMessage httpResponseMessage =
				await _httpClient.DeleteAsync("/api/patient/" + Guid.Empty);
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		[Fact]
		public async Task TestDeletePatientInvalidId()
		{
			//Act
			HttpResponseMessage httpResponseMessage =
				await _httpClient.DeleteAsync("/api/patient/" + Guid.NewGuid());
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status404NotFound, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestGetPatientContact()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient/" +
				TestUtilities.Patient.Id + "/contact");

			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status200OK, (int)httpResponseMessage.StatusCode);
			Assert.Equal("application/json; charset=utf-8", httpResponseMessage.Content.Headers.ContentType.ToString());
			Assert.Equal(JsonSerializer.Serialize(TestUtilities.PatientContact), httpResponseMessage.Content.ReadAsStringAsync().Result, true);
		}

		[Fact]
		public async Task TestGetPatientContactEmptyId()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient/" + 
				Guid.Empty + "/contact");
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		
		[Fact]
		public async Task TestGetPatientContactInvalidId()
		{
			// Act
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient/" + 
				Guid.NewGuid() + "/contact");
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status404NotFound, (int)httpResponseMessage.StatusCode);
		}
		
		[Fact]
		public async Task TestGetPatientContactNoContact()
		{
			// Arrange
			await _httpClient.PostAsync("/api/patient/", 
				new StringContent(
					JsonSerializer.Serialize(new Patient()
					{
						Id = Guid.NewGuid(),
						FirstName = "John",
						LastName = "Smith",
						LastFourOfSSN = "1234",
						DateOfBirth = DateTime.Now.AddDays(-7),
					}), Encoding.UTF8, "application/json"));
		
			// Act	
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("/api/patient/" + 
			    Guid.NewGuid() + "/contact");
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status404NotFound, (int)httpResponseMessage.StatusCode);
		}
		
		[Fact]
		public async Task TestCreatePatientContact()
		{
			// TODO: Fix status 500 error
			
			// Arrange
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Smith",
				LastFourOfSSN = "1234",
				DateOfBirth = DateTime.Now.AddDays(-7),
			};
			
			HttpResponseMessage response = _httpClient.PostAsync("/api/patient/", 
				new StringContent(JsonSerializer.Serialize(patient), Encoding.UTF8, "application/json")).Result;
			
			// Act	
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/" + patient.Id + "/contact", 
				new StringContent(JsonSerializer.Serialize(new PatientContact()
				{
					Patient = patient,
					PatientId = patient.Id,
					PhoneNumber = "111-111-1111",
					EmailAddress = "johnsmith@gmail.com",
				}), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		[Fact]
		public async Task TestCreatePatientContactEmptyId()
		{
			// Arrange
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Smith",
				LastFourOfSSN = "1234",
				DateOfBirth = DateTime.Now.AddDays(-7),
			};
			
			await _httpClient.PostAsync("/api/patient/", 
				new StringContent(JsonSerializer.Serialize(patient), Encoding.UTF8, "application/json"));
			
			// Act	
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/" + Guid.Empty + "/contact", 
				new StringContent(JsonSerializer.Serialize(new PatientContact()
				{
					Patient = patient,
					PatientId = patient.Id,
					PhoneNumber = "111-111-1111",
					EmailAddress = "johnsmith@gmail.com",
				}), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
		
		[Fact]
		public async Task TestCreatePatientContactInvalidId()
		{
			// Arrange
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Smith",
				LastFourOfSSN = "1234",
				DateOfBirth = DateTime.Now.AddDays(-7),
			};
			
			await _httpClient.PostAsync("/api/patient/", 
				new StringContent(JsonSerializer.Serialize(patient), Encoding.UTF8, "application/json"));
			
			// Act	
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/" + Guid.NewGuid() + "/contact", 
				new StringContent(JsonSerializer.Serialize(new PatientContact()
				{
					Patient = patient,
					PatientId = patient.Id,
					PhoneNumber = "111-111-1111",
					EmailAddress = "johnsmith@gmail.com",
				}), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status404NotFound, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestCreatePatientContactType()
		{
			// Arrange
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Smith",
				LastFourOfSSN = "1234",
				DateOfBirth = DateTime.Now.AddDays(-7),
			};
			
			await _httpClient.PostAsync("/api/patient/", 
				new StringContent(JsonSerializer.Serialize(patient), Encoding.UTF8, "application/json"));
			
			// Act	
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/" + Guid.NewGuid() + "/contact", 
				new StringContent(JsonSerializer.Serialize(25), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}

		[Fact]
		public async Task TestCreatePatientContactAlreadyExist()
		{	
			// Act	
			HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("/api/patient/" + TestUtilities.Patient.Id + "/contact", 
				new StringContent(JsonSerializer.Serialize(TestUtilities.PatientContact), Encoding.UTF8, "application/json"));
			
			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(StatusCodes.Status400BadRequest, (int)httpResponseMessage.StatusCode);
		}
	}
	
	// TODO: Finish integration tests for PatientService
}
