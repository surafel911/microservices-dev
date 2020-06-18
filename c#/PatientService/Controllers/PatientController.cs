using System;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

using PatientService.Models;
using PatientService.Services;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PatientController : ControllerBase
    {
		private readonly ILogger<PatientController> _logger;
		private readonly IPatientDbService _patientDbService;
		private readonly IHostApplicationLifetime _hostApplicationLifetime;
        
        public PatientController(ILogger<PatientController> logger,
			IPatientDbService patientDbService,
			IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
			_patientDbService = patientDbService;
			_hostApplicationLifetime = hostApplicationLifetime;
		}

		[HttpGet("health")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IActionResult GetHealth()
		{
			try {
				_patientDbService.CanConnect();
			} catch (Exception e) {
				_logger.LogCritical(e, "An error occured testing database connection.");
				_hostApplicationLifetime.StopApplication();
			}

			return Ok("Service healthy.");
		}

		[HttpGet("{id}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetPatient(Guid id)
		{
			Patient patient = null;

			if (id == null || id == Guid.Empty) {
				return BadRequest("Invalid guid.");
			}

			patient = _patientDbService.FindPatient(id);

			if (patient == null) {
				return NotFound("Patient not found.");
			}

			return Ok(patient);
		}

        [HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPatient(
			[FromQuery] string firstName,
			[FromQuery] string lastName,
			[FromQuery] DateTime dateOfBirth)
        {
			Patient patient = null;

			if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
					dateOfBirth == null) {
				return BadRequest("Invalid request parameter.");
			}

			patient = _patientDbService.FindPatient(firstName, lastName, dateOfBirth);

			if (patient == null) {
				return NotFound("Patient not found.");
			}

			return Ok(patient);
        }

		[HttpPost]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult CreatePatient(
			[FromBody] Patient patient)
		{
			if (!ModelState.IsValid) {
				return BadRequest("Patient data is invalid.");
			}

			if (_patientDbService.FindPatient(patient.FirstName,
				patient.LastName, patient.DateOfBirth) != null) {
				return BadRequest("Patient already exists.");
			}

			_patientDbService.AddPatient(patient);

			return NoContent();
		}

		[HttpPut("{id}")]
		[Consumes("application/json-patch+json")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		public ActionResult<Patient> UpdatePatient(
			Guid id,
			[FromBody] Patient patientDTO)
		{
			Patient patient = null;

			if (id == null || id == Guid.Empty) {
				return BadRequest("Invalid guid.");
			}

			patient = _patientDbService.FindPatient(id);
			if (patient == null) {
				return NotFound();
			}

			// TODO: Do a patch of the patient
			patient = patientDTO;
			patient.Id = id;

			_patientDbService.UpdatePatient(patient);

			return Ok(patient);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeletePatient(Guid id)
		{
			Patient patient = null;

			if (id == null || id == Guid.Empty) {
				return BadRequest("Invalid guid.");
			}

			patient = _patientDbService.FindPatient(id);
			if (patient == null) {
				return NotFound("Patient not found.");
			}

			_patientDbService.RemovePatient(patient);

			return NoContent();
		}

		[HttpGet("{id}/contact")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PatientContact> GetPatientContact(Guid id)
        {
			// TODO: Check if patient and patient contact are both valid

			PatientContact patientContact = null;

			if (id == Guid.Empty) {
				return BadRequest("Empty guid.");
			}

			patientContact = _patientDbService.FindPatientContact(id);
			if (patientContact == null) {
				return NotFound("Patient contact info not found.");
			}

			return Ok(patientContact);
        }
		
		[HttpPost("{id}/contact")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult CreatePatientContact(
			Guid id,
			[FromBody] PatientContact patientContact)
		{
			// TODO: Finish this implementation
			// TODO: Check if patient and patient contact are both valid

			if (id == Guid.Empty) {
				return BadRequest("Empty guid.");
			}

			if (_patientDbService.FindPatientContact(id) != null) {
				return BadRequest("Patient already exists.");
			}

			_patientDbService.AddPatientContact(patientContact);

			return NoContent();
		}

		[HttpPut("{id}/contact")]
		[Consumes("application/json-patch+json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		public IActionResult UpdatePatientContact(
			Guid id,
			[FromBody] PatientContact patientContact)
		{
			// TODO: Finish this implementation
			// TODO: Check if patient and patient contact are both valid

			if (id == Guid.Empty) {
				return BadRequest("Empty guid.");
			}

			return NoContent();
		}

		[HttpDelete("{id}/contact")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeletePatientContact(Guid id)
		{
			// TODO: Finish this implementation
			// TODO: Check if patient and patient contact are both valid

			PatientContact patientContact = null;

			if (id == Guid.Empty) {
				return BadRequest("Empty guid.");
			}

			patientContact = _patientDbService.FindPatientContact(id);
			if (patientContact == null) {
				return NotFound("Patient contact info not found.");
			}

			_patientDbService.RemovePatientContact(patientContact);

			return NoContent();
		}
    }
}
