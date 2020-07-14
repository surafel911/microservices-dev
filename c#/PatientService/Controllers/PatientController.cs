using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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

        public PatientController(
			ILogger<PatientController> logger,
			IPatientDbService patientDbService)
        {
            _logger = logger;
			_patientDbService = patientDbService;
		}

		[HttpGet("{id}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetPatient([GuidNotEmpty] Guid id)
		{
			Patient patient = null;

			patient = _patientDbService.FindPatient(id);
			if (patient == null) {
				return Problem("Patient not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			return Ok(patient);
		}

        [HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPatient(
			[Required] [StringLength(40)] [RegularExpression(@"^[a-zA-Z''-'\s]*$")] [FromQuery] string firstName,
			[Required] [StringLength(40)] [RegularExpression(@"^[a-zA-Z''-'\s]*$")] [FromQuery] string lastName,
			[Required] [FromQuery] DateTime dateOfBirth)
        {
			Patient patient = null;

			patient = _patientDbService.FindPatient(firstName, lastName, dateOfBirth);
			if (patient == null) {
				return Problem("Patient not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
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
			if (_patientDbService.FindPatient(patient.FirstName,
				patient.LastName, patient.DateOfBirth) != null) {
				return Problem("Patient already exists.", default, StatusCodes.Status400BadRequest,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			_patientDbService.AddPatient(patient);

			return NoContent();
		}

		[HttpPatch("{id}")]
		[Consumes("application/json-patch+json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		public IActionResult UpdatePatient(
			[GuidNotEmpty] Guid id,
			[FromBody] Patient patientDTO)
		{
			Patient patient = null;

			patient = _patientDbService.FindPatient(patientDTO.Id);
			if (patient == null) {
				return Problem("Patient not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			// TODO: Do a patch of the patient
			patient = patientDTO;

			_patientDbService.UpdatePatient(patient);

			return NoContent();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeletePatient([GuidNotEmpty] Guid id)
		{
			Patient patient = null;

			patient = _patientDbService.FindPatient(id);
			if (patient == null) {
				return Problem("Patient not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			_patientDbService.RemovePatient(patient);

			return NoContent();
		}

		[HttpGet("{id}/contact")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPatientContact([GuidNotEmpty] Guid id)
        {
			PatientContact patientContact = null;

			if (_patientDbService.FindPatient(id) == null) {
				return Problem("Patient not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			patientContact = _patientDbService.FindPatientContact(id);
			if (patientContact == null) {
				return Problem("Patient contact not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			return Ok(patientContact);
        }

		[HttpPost("{id}/contact")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult CreatePatientContact(
			[GuidNotEmpty] Guid id,
			[FromBody] PatientContact patientContact)
		{
			if (_patientDbService.FindPatient(id) == null) {
				return Problem("Patient not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			if (_patientDbService.FindPatientContact(id) != null) {
				return Problem("Patient contact already exists.", default, StatusCodes.Status400BadRequest,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			_patientDbService.AddPatientContact(patientContact);

			return NoContent();
		}

		[HttpPatch("{id}/contact")]
		[Consumes("application/json-patch+json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		public IActionResult UpdatePatientContact(
			[GuidNotEmpty] Guid id,
			[FromBody] PatientContact patientContactDTO)
		{
			PatientContact patientContact = null;

			if (_patientDbService.FindPatient(id) == null) {
				return Problem("Patient not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			patientContact = _patientDbService.FindPatientContact(id);
			if (patientContact == null) {
				return Problem("Patient contact not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			patientContact = patientContactDTO;
			patientContact.PatientId = id;

			_patientDbService.UpdatePatientContact(patientContact);

			return NoContent();
		}

		[HttpDelete("{id}/contact")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeletePatientContact([GuidNotEmpty] Guid id)
		{
			PatientContact patientContact = null;

			if (_patientDbService.FindPatient(id) == null) {
				return Problem("Patient not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			patientContact = _patientDbService.FindPatientContact(id);
			if (patientContact == null) {
				return Problem("Patient contact not found.", default, StatusCodes.Status404NotFound,
					"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
			}

			_patientDbService.RemovePatientContact(patientContact);

			return NoContent();
		}
    }
}
