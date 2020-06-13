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
    public class PatientServiceTest : IDisposable
    {
		private readonly Patient _patient;
		private readonly ILogger<PatientController> _mockLogger;
		private readonly IHostApplicationLifetime _mockHostApplicationLifetime;
		private readonly IPatientServiceDbService _mockPatientServiceDbService;

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
			_mockHostApplicationLifetime = new MockHostApplicationLifetime();
			_mockPatientServiceDbService = new MockPatientServiceDbService();
			
			_mockPatientServiceDbService.AddPatient(_patient);
		}
	
		public void Dispose()
		{
		}

		[Fact]
		public void TestGetHealth()
		{
			PatientController patientController = new PatientController(
				_mockLogger,
				_mockHostApplicationLifetime,
				_mockPatientServiceDbService
			);

			// Act
			IActionResult result = patientController.GetHealth();

			// Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<string>(((OkObjectResult)result).Value);
		}

		[Fact]
		public void TestGetPatientFromIdValidId()
		{
			PatientController patientController = new PatientController(
				_mockLogger,
				_mockHostApplicationLifetime,
				_mockPatientServiceDbService
			);

			// Act
			IActionResult result = patientController.GetPatient(_patient.Id);

			// Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.Equal(_patient, ((OkObjectResult)result).Value);
		}

		[Fact]
		public void TestGetPatientFromIdEmptyId()
		{
			PatientController patientController = new PatientController(
				_mockLogger,
				_mockHostApplicationLifetime,
				_mockPatientServiceDbService
			);

			// Act
			IActionResult result = patientController.GetPatient(Guid.Empty);

			// Assert
			Assert.IsType<BadRequestObjectResult>(result);
			Assert.IsType<string>(((BadRequestObjectResult)result).Value);
		}

		[Fact]
		public void TestGetPatientFromIdPatientNotFound()

		{
			PatientController patientController = new PatientController(
				_mockLogger,
				_mockHostApplicationLifetime,
				_mockPatientServiceDbService
			);

			// Act
			IActionResult result = patientController.GetPatient(Guid.NewGuid());

			// Assert
			Assert.IsType<NotFoundObjectResult>(result);
			Assert.IsType<string>(((NotFoundObjectResult)result).Value);
		}

        [Fact]
        public void TestGetPatientFromQueryValidQuery()
		{
			PatientController patientController = new PatientController(
				_mockLogger,
				_mockHostApplicationLifetime,
				_mockPatientServiceDbService
			);

			// Act
			IActionResult result = patientController.GetPatient(
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
			PatientController patientController = new PatientController(
				_mockLogger,
				_mockHostApplicationLifetime,
				_mockPatientServiceDbService
			);

			// Act
			IActionResult result = patientController.GetPatient(
				value, value, DateTime.Now);

			// Assert
			Assert.IsType<BadRequestObjectResult>(result);
			Assert.IsType<string>(((BadRequestObjectResult)result).Value);
		}

		[Fact]
		public void TestGetPatientFromQueryPatientNotFound()
		{
			PatientController patientController = new PatientController(
				_mockLogger,
				_mockHostApplicationLifetime,
				_mockPatientServiceDbService
			);

			// Act
			IActionResult result = patientController.GetPatient(
				_patient.FirstName, _patient.LastName, DateTime.Now);

			// Assert
			Assert.IsType<NotFoundObjectResult>(result);
			Assert.IsType<string>(((NotFoundObjectResult)result).Value);
		}
    }
}