using System;

using Xunit;

using Microsoft.AspNetCore.Mvc;
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
		private readonly ILogger<PatientController> _mockLogger;
		private readonly IPatientDbService _mockPatientDbService;
		private readonly IHostApplicationLifetime _mockHostApplicationLifetime;
		private readonly PatientController _patientController;

		public PatientServiceTest()
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

			_mockLogger = new NullLogger<PatientController>();
			_mockPatientDbService = new MockPatientDbService();
			_mockHostApplicationLifetime = new MockHostApplicationLifetime();
			
			_mockPatientDbService.AddPatient(_patient);
			
			_patientController = new PatientController(
				_mockLogger,
				_mockPatientDbService,
				_mockHostApplicationLifetime
			);
		}
	
		public void Dispose()
		{
		}

		// TODO: Add tests to make sure that the error strings are not empty.
		// TODO: Add tests that look into mock db and check if patients are really there.

		[Fact]
		public void TestGetHealth()
		{
			// Act
			IActionResult result = _patientController.GetHealth();

			// Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<string>(((OkObjectResult)result).Value);
		}

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
		public void TestGetPatientFromIdEmptyId()
		{
			// Act
			IActionResult result = _patientController.GetPatient(Guid.Empty);

			// Assert
			Assert.IsType<BadRequestObjectResult>(result);
			Assert.IsType<string>(((BadRequestObjectResult)result).Value);
		}

		[Fact]
		public void TestGetPatientFromIdPatientNotFound()

		{
			// Act
			IActionResult result = _patientController.GetPatient(Guid.NewGuid());

			// Assert
			Assert.IsType<NotFoundObjectResult>(result);
			Assert.IsType<string>(((NotFoundObjectResult)result).Value);
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

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void TestGetPatientFromQueryNullQuery(string value)
		{
			// Act
			IActionResult result = _patientController.GetPatient(
				value, value, DateTime.Now);

			// Assert
			Assert.IsType<BadRequestObjectResult>(result);
			Assert.IsType<string>(((BadRequestObjectResult)result).Value);
		}

		[Fact]
		public void TestGetPatientFromQueryPatientNotFound()
		{
			// Act
			IActionResult result = _patientController.GetPatient(
				_patient.FirstName, _patient.LastName, DateTime.Now);

			// Assert
			Assert.IsType<NotFoundObjectResult>(result);
			Assert.IsType<string>(((NotFoundObjectResult)result).Value);
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
		} 

		[Fact]
		public void TestCreatePatientNullPatient()
		{
			// Act
			IActionResult result = _patientController.CreatePatient(null);

			// Assert
			Assert.IsType<BadRequestObjectResult>(result);
			Assert.IsType<string>(((BadRequestObjectResult)result).Value);
		}

		[Fact]
		public void TestCreatePatientAlreadyExists()
		{
			// Act
			IActionResult result = _patientController.CreatePatient(_patient);

			// Assert
			Assert.IsType<BadRequestObjectResult>(result);
			Assert.IsType<string>(((BadRequestObjectResult)result).Value);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void TestCreatePatientInvalidFirstName(string value)
		{
			// Act
			IActionResult result = _patientController.CreatePatient(new Patient {
				FirstName = value,
				LastName = "Johnson",
				LastFourOfSSN = "5678",
				DateOfBirth = DateTime.Now
			});

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void TestCreatePatientInvalidLastName(string value)
		{
			// Act
			IActionResult result = _patientController.CreatePatient(new Patient {
				FirstName = "Adam",
				LastName = value,
				LastFourOfSSN = "5678",
				DateOfBirth = DateTime.Now
			});

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void TestCreatePatientInvalidLastFourOfSSN(string value)
		{
			// Act
			IActionResult result = _patientController.CreatePatient(new Patient {
				FirstName = "Adam",
				LastName = "Johnson",
				LastFourOfSSN = value,
				DateOfBirth = DateTime.Now
			});

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}
    }
}
