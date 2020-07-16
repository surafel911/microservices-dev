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

using PatientServiceTest.Models;

namespace PatientServiceTest
{
    public class PatientServiceUnitTest : IDisposable
    {
		private readonly Patient _patient;
		private readonly PatientController _patientController;
		private readonly MockPatientDbService _mockPatientDbService;

		public PatientServiceUnitTest()
		{
			// Setup
			_patient = new Patient
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Smith",
				LastFourOfSSN = "1234",
				DateOfBirth = DateTime.Now.AddDays(-7),
			};

			_mockPatientDbService = new MockPatientDbService();
			_mockPatientDbService.AddPatient(_patient);

			_patientController = new PatientController(
				new NullLogger<PatientController>(),
				(IPatientDbService)_mockPatientDbService
			);

			_patientController.ProblemDetailsFactory = new MockProblemDetailsFactory();
		}

		public void Dispose()
		{
		}

		// TODO: Add tests to make sure that the error strings are not empty.
		// TODO: Add tests that look into mock db and check if patients are really there.

		[Fact]
		public void TestGetPatientFromIdValidId()
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
			Assert.NotNull(((ObjectResult)result).Value);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}

        [Fact]
        public void TestGetPatientFromQueryValidQuery()
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
		public void TestCreatePatientValidPatient()
		{
			// Act
			IActionResult result = _patientController.CreatePatient(new Patient {
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
		public void TestUpdatePatientValidPatient()
		{
			// Act
			string firstName = "Jacob", lastName = "Dunham";
			Patient patient = _patient;

			patient.FirstName = firstName;
			patient.LastName = lastName;

			IActionResult result = _patientController.UpdatePatient(patient.Id, patient);

			// Assert
			patient = _mockPatientDbService.PatientList.Find(p => p.Id == patient.Id);

			Assert.IsType<NoContentResult>(result);
			Assert.Equal(patient.FirstName, firstName);
			Assert.Equal(patient.LastName, lastName);
		}

		[Fact]
		public void TestUpdatePatientInvalidPatient()
		{
			// Act
			Patient patient = new Patient();

			patient.Id = new Guid();
			patient.FirstName = "Jacob";
			patient.LastName = "Dunham";
			patient.DateOfBirth = _patient.DateOfBirth;
			patient.LastFourOfSSN = _patient.LastFourOfSSN;

			IActionResult result = _patientController.UpdatePatient(patient.Id, patient);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}

		[Fact]
		public void TestDeletePatientValidPatient()
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
			Patient patient = new Patient();

			patient.Id = new Guid();
			patient.FirstName = _patient.FirstName;
			patient.LastName = _patient.LastName;
			patient.DateOfBirth = _patient.DateOfBirth;
			patient.LastFourOfSSN = _patient.LastFourOfSSN;

			IActionResult result = _patientController.DeletePatient(patient.Id);

			// Assert
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ProblemDetails>(((ObjectResult)result).Value);
			Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
		}
    }
}
