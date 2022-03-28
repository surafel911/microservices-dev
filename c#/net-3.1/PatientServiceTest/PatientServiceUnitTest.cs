using System;
using Microsoft.AspNetCore.Http;
using Xunit;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using PatientService.Models;
using PatientService.Services;
using PatientService.Controllers;
using DataAtThePointOfCare.Models;
using DataAtThePointOfCare.Services;

using PatientServiceTest.Models;

namespace PatientServiceTest
{
    public class PatientServiceUnitTest : IDisposable
    {
		private readonly Patient _patient;
		private readonly PatientContact _patientContact;
		private readonly PatientController _patientController;
		private readonly MockPatientDbService _mockPatientDbService;

		public PatientServiceUnitTest()
		{
			// Setup
			_patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Smith",
				LastFourOfSSN = "1234",
				DateOfBirth = DateTime.Now.AddDays(-7),
			};

			_patientContact = new PatientContact()
			{
				Patient = _patient,
				PatientId = _patient.Id,
				PhoneNumber = "666-666-6666",
			};

			_mockPatientDbService = new MockPatientDbService();
			_mockPatientDbService.AddPatient(_patient);
			_mockPatientDbService.AddPatientContact(_patientContact);

			_patientController = new PatientController(
				new NullLogger<PatientController>(),
				_mockPatientDbService
			);

			_patientController.ProblemDetailsFactory = new MockProblemDetailsFactory();
		}

		public void Dispose()
		{
		}

		// TODO: Add tests to make sure that the error strings are not empty.
		// TODO: Add tests that look into mock db and check if patients are really there.

		[Fact]
		public void TestGetPatientFromId()
		{
			// Act
			IActionResult result = _patientController.GetPatient(_patient.Id);

			// Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.Equal(_patient, ((OkObjectResult)result).Value);
		}

		[Fact]
		public void TestGetPatientFromIdPatientNotFound()

		{
			// Act
			IActionResult result = _patientController.GetPatient(Guid.NewGuid());

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}

        [Fact]
        public void TestGetPatientFromQuery()
		{
			// Act
			IActionResult result = _patientController.GetPatient(
				_patient.FirstName, _patient.LastName, _patient.DateOfBirth);

			// Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.Equal(_patient, ((OkObjectResult)result).Value);
        }

		[Fact]
		public void TestGetPatientFromQueryPatientNotFound()
		{
			// Act
			IActionResult result = _patientController.GetPatient(
				_patient.FirstName, _patient.LastName, DateTime.Now);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}

		[Fact]
		public void TestCreatePatient()
		{
			// Act
			IActionResult result = _patientController.CreatePatient(new Patient() {
				FirstName = "Adam",
				LastName = "Johnson",
				LastFourOfSSN = "5678",
				DateOfBirth = DateTime.Now
			});

			// Assert
			Assert.IsType<NoContentResult>(result);
			Assert.Contains(_mockPatientDbService.PatientList, patient => patient == _patient);
		}

		[Fact]
		public void TestCreatePatientAlreadyExists()
		{
			// Act
			IActionResult result = _patientController.CreatePatient(_patient);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
		}

		[Fact]
		public void TestUpdatePatient()
		{
			// Act
			string firstName = "Jacob", lastName = "Dunham";

			PatientDTO patientDTO = new PatientDTO()
			{
				FirstName = firstName,
				LastName = lastName,
				DateOfBirth = _patient.DateOfBirth,
				LastFourOfSSN = _patient.LastFourOfSSN,
			};

			IActionResult result = _patientController.UpdatePatient(_patient.Id, patientDTO);

			// Assert
			Patient patient = _mockPatientDbService.PatientList.Find(p => p.Id == _patient.Id);

			Assert.IsType<NoContentResult>(result);
			Assert.Equal(patient.FirstName, firstName);
			Assert.Equal(patient.LastName, lastName);
		}

		[Fact]
		public void TestUpdatePatientInvalidPatient()
		{
			// Act
			PatientDTO patientDTO = new PatientDTO()
			{
				FirstName = "Jacob",
				LastName = "Dunham",
				DateOfBirth = _patient.DateOfBirth,
				LastFourOfSSN = _patient.LastFourOfSSN,
			};

			IActionResult result = _patientController.UpdatePatient(Guid.NewGuid(), patientDTO);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
		}

		[Fact]
		public void TestDeletePatient()
		{
			// Act
			IActionResult result = _patientController.DeletePatient(_patient.Id);

			// Assert
			Assert.IsType<NoContentResult>(result);
			Assert.DoesNotContain(_mockPatientDbService.PatientList, patient => patient == _patient);
		}

		[Fact]
		public void TestDeletePatientDoesNotExist()
		{
			// Act
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = _patient.FirstName,
				LastName = _patient.LastName,
				DateOfBirth = _patient.DateOfBirth,
				LastFourOfSSN = _patient.LastFourOfSSN,
			};

			IActionResult result = _patientController.DeletePatient(patient.Id);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}

		[Fact]
		public void TestGetPatientContactValidPatientContact()
		{
			// Act
			IActionResult result = _patientController.GetPatientContact(_patient.Id);

			// Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<PatientContact>(((OkObjectResult)result).Value);
			Assert.Equal(((OkObjectResult)result).Value, _patientContact);
		}

		[Fact]
		public void TestGetPatientContactInvalidPatient()
		{
			// Act
			IActionResult result = _patientController.GetPatientContact(new Guid());

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}

		[Fact]
		public void TestGetPatientContactInvalidPatientContact()
		{
			// Act
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = _patient.FirstName,
				LastName = _patient.LastName,
				DateOfBirth = _patient.DateOfBirth,
				LastFourOfSSN = _patient.LastFourOfSSN,
			};

			_mockPatientDbService.AddPatient(patient);

			IActionResult result = _patientController.GetPatientContact(patient.Id);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}

		[Fact]
		public void TestCreatePatientContactValidContact()
		{
			// Act
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = _patient.FirstName,
				LastName = _patient.LastName,
				DateOfBirth = _patient.DateOfBirth,
				LastFourOfSSN = _patient.LastFourOfSSN,
			};

			PatientContact patientContact = new PatientContact()
			{
				Patient = patient,
				PatientId = patient.Id,
				PhoneNumber = "666-666-6666",
			};

			_mockPatientDbService.AddPatient(patient);

			IActionResult result = _patientController.CreatePatientContact(patient.Id, patientContact);

			// Assert
			Assert.IsType<NoContentResult>(result);
			Assert.Contains(_mockPatientDbService.PatientContactList, patientContact => patientContact.PatientId == patient.Id);
		}

		[Fact]
		public void TestCreatePatientContactInvalidPatient()
		{
			// Act
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = "Jacob",
				LastName = "Johnson",
				DateOfBirth = DateTime.Now,
				LastFourOfSSN = "1234",
			};

			PatientContact patientContact = new PatientContact()
			{
				Patient = patient,
				PatientId = patient.Id,
				PhoneNumber = "666-666-6666",
			};

			IActionResult result = _patientController.CreatePatientContact(patient.Id, patientContact);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
			Assert.DoesNotContain(_mockPatientDbService.PatientContactList, pc => pc == patientContact);
		}

		[Fact]
		public void TestCreatePatientContactAlreadyExists()
		{
			// Act
			PatientContact patientContact = new PatientContact()
			{
				Patient = _patient,
				PatientId = _patient.Id,
				PhoneNumber = "666-666-6666",
			};

			IActionResult result = _patientController.CreatePatientContact(_patient.Id, patientContact);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
		}

		[Fact]
		public void TestUpdatePatientContactValidContact()
		{
			// Act
			string phoneNumber = "111-111-1111";

			PatientContactDTO patientContactDTO = new PatientContactDTO()
			{
				PhoneNumber = phoneNumber,
			};

			IActionResult result = _patientController.UpdatePatientContact(_patient.Id, patientContactDTO);

			// Assert
			PatientContact patientContact = _mockPatientDbService.PatientContactList.Find(p => p.PatientId == _patient.Id);

			Assert.IsType<NoContentResult>(result);
			Assert.Equal(_patientContact, patientContact);
		}

		[Fact]
		public void TestUpdatePatientContactInvalidPatient()
		{
			// Act
			PatientContactDTO patientContactDTO = new PatientContactDTO()
			{
				PhoneNumber = "111-111-1111",
			};

			IActionResult result = _patientController.UpdatePatientContact(Guid.NewGuid(), patientContactDTO);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
			Assert.NotEqual(_patientContact.PhoneNumber, patientContactDTO.PhoneNumber);
		}

		[Fact]
		public void TestUpdatePatientContactInvalidContact()
		{
			// Act
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = "Jack",
				LastName = "Johnson",
				DateOfBirth = DateTime.Now,
				LastFourOfSSN = "1234",
			};

			PatientContactDTO patientContactDTO = new PatientContactDTO()
			{
				PhoneNumber = "111-111-1111",
			};

			_mockPatientDbService.AddPatient(patient);

			IActionResult result = _patientController.UpdatePatientContact(patient.Id, patientContactDTO);

			// Assert
			PatientContact patientContact = _mockPatientDbService.PatientContactList.Find(p => p.PatientId == patient.Id);

			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
			Assert.NotEqual(_patientContact, patientContact);
		}

		[Fact]
		public void TestDeletePatientContactValidContact()
		{
			// Act
			IActionResult result = _patientController.DeletePatientContact(_patient.Id);
			
			// Assert
			Assert.IsType<NoContentResult>(result);
			Assert.DoesNotContain(_mockPatientDbService.PatientContactList, patientContact => patientContact == _patientContact);
		}

		[Fact]
		public void TestDeletePatientContactInvalidPatient()
		{
			// Act
			IActionResult result = _patientController.DeletePatientContact(new Guid());
			
			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}
		
		[Fact]
		public void TestDeletePatientContactInvalidContact()
		{
			// Act
			Patient patient = new Patient()
			{
				Id = Guid.NewGuid(),
				FirstName = "Jack",
				LastName = "Johnson",
				DateOfBirth = DateTime.Now,
				LastFourOfSSN = "1234",
			};

			_mockPatientDbService.AddPatient(patient);
			
			IActionResult result = _patientController.DeletePatientContact(patient.Id);
			
			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}
    }
}
