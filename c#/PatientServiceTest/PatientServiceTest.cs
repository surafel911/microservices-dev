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
    public class PatientServiceTest
    {
		private readonly Patient _patient;

		public PatientServiceTest()
		{
			_patient = new Patient
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Smith",
				LastFourOfSSN = "1234",
				DateOfBirth = DateTime.Now.AddDays(-7),
			};
		}

		// TODO: https://xunit.net/docs/shared-context

		[Fact]
		public void TestGetHealth()
		{
			ILogger<PatientController> mockLogger =
				new NullLogger<PatientController>();
			IHostApplicationLifetime mockHostApplicationLifetime = 
				new MockHostApplicationLifetime();
			IPatientServiceDbService mockPatientServiceDbService = 
				new MockPatientServiceDbService();
			
			PatientController patientController = new PatientController(
				mockLogger,
				mockHostApplicationLifetime,
				mockPatientServiceDbService
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
			// Setup
			ILogger<PatientController> mockLogger =
				new NullLogger<PatientController>();
			IHostApplicationLifetime mockHostApplicationLifetime = 
				new MockHostApplicationLifetime();
			IPatientServiceDbService mockPatientServiceDbService = 
				new MockPatientServiceDbService();

			mockPatientServiceDbService.AddPatient(_patient);
			
			PatientController patientController = new PatientController(
				mockLogger,
				mockHostApplicationLifetime,
				mockPatientServiceDbService
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
			// Setup
			ILogger<PatientController> mockLogger =
				new NullLogger<PatientController>();
			IHostApplicationLifetime mockHostApplicationLifetime = 
				new MockHostApplicationLifetime();
			IPatientServiceDbService mockPatientServiceDbService = 
				new MockPatientServiceDbService();
			
			PatientController patientController = new PatientController(
				mockLogger,
				mockHostApplicationLifetime,
				mockPatientServiceDbService
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
			// Setup
			ILogger<PatientController> mockLogger =
				new NullLogger<PatientController>();
			IHostApplicationLifetime mockHostApplicationLifetime = 
				new MockHostApplicationLifetime();
			IPatientServiceDbService mockPatientServiceDbService = 
				new MockPatientServiceDbService();
			
			mockPatientServiceDbService.AddPatient(_patient);
			
			PatientController patientController = new PatientController(
				mockLogger,
				mockHostApplicationLifetime,
				mockPatientServiceDbService
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
			// Setup
			ILogger<PatientController> mockLogger =
				new NullLogger<PatientController>();
			IHostApplicationLifetime mockHostApplicationLifetime = 
				new MockHostApplicationLifetime();
			IPatientServiceDbService mockPatientServiceDbService = 
				new MockPatientServiceDbService();
			
			mockPatientServiceDbService.AddPatient(_patient);
			
			PatientController patientController = new PatientController(
				mockLogger,
				mockHostApplicationLifetime,
				mockPatientServiceDbService
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
			// Setup
			ILogger<PatientController> mockLogger =
				new NullLogger<PatientController>();
			IHostApplicationLifetime mockHostApplicationLifetime = 
				new MockHostApplicationLifetime();
			IPatientServiceDbService mockPatientServiceDbService = 
				new MockPatientServiceDbService();
			
			mockPatientServiceDbService.AddPatient(_patient);
			
			PatientController patientController = new PatientController(
				mockLogger,
				mockHostApplicationLifetime,
				mockPatientServiceDbService
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
			// Setup
			ILogger<PatientController> mockLogger =
				new NullLogger<PatientController>();
			IHostApplicationLifetime mockHostApplicationLifetime = 
				new MockHostApplicationLifetime();
			IPatientServiceDbService mockPatientServiceDbService = 
				new MockPatientServiceDbService();
			
			mockPatientServiceDbService.AddPatient(_patient);
			
			PatientController patientController = new PatientController(
				mockLogger,
				mockHostApplicationLifetime,
				mockPatientServiceDbService
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
