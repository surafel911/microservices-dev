using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using PatientService.Data;
using PatientService.Models;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
		private readonly PatientServiceDbContext _patientServiceDbContext;
        
        public PatientController(ILogger<PatientController> logger, PatientServiceDbContext patientServiceDbContext)
        {
            _logger = logger;
			_patientServiceDbContext = patientServiceDbContext;
        }

        [HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPatient(
			[FromQuery] string FirstName,
			[FromQuery] string LastName,
			[FromQuery] DateTime DateOfBirth)
        {
			return Ok("Got patient.");
        }

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetPatientId(
			Guid id)
		{
			return Ok("Got patient.");
		}

		[HttpPost]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult CreatePatient(
			[FromBody] Patient patient)
		{
			return Ok("Created patient.");
		}

		[HttpPatch("{id}")]
		[Consumes("application/json-patch+json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		public IActionResult UpdatePatient(
			Guid id,
			[FromBody] Patient patient)
		{
			return Ok("Updated patient.");
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeletePatient(Guid id)
		{
			return Ok("Deleted patient.");
		}

		[HttpGet("{id}/contact")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPatientContact(Guid id)
        {
			return Ok("Got patient contact.");
        }
		
		[HttpPost("{id}/contact")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult CreatePatientContact(
			Guid id)
		{
			return Ok("Created patient contact.");
		}

		[HttpPatch("{id}/contact")]
		[Consumes("application/json-patch+json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		public IActionResult UpdatePatientContact(
			Guid id,
			[FromBody] PatientContact patientContact)
		{
			return Ok("Updated patient contact.");
		}

		[HttpDelete("{id}/contact")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeletePatientContact(Guid id)
		{
			return Ok("Deleted patient contact.");
		}
    }
}
