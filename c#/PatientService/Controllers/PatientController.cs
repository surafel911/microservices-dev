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

			if (id == Guid.Empty) {
				return BadRequest("Invalid guid.");
			}

			patient = _patientDbService.FindPatient(id);

			if (patient == null) {
				return NotFound();
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

			if (id == Guid.Empty) {
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

			if (id == Guid.Empty) {
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
        public ActionResult<PatientContact> GetPatientContact([GuidNotEmpty] Guid id)
        {
			// TODO: Check if patient and patient contact are both valid

			Patient patient = null;
			PatientContact patientContact = null;

			if (id == Guid.Empty) {
				return BadRequest("Empty guid.");
			}

			patient = _patientDbService.FindPatient(id);
			if (patient == null) {
				return BadRequest("");
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
