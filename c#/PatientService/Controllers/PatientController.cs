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

		private ObjectResult ReturnProblem(string detail, int status)
		{
			return Problem(detail, string.Empty, status,
				"An error occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1");
		}

        public PatientController(
			ILogger<PatientController> logger,
			IPatientDbService patientDbService)
        {
            _logger = logger;
			_patientDbService = patientDbService;
		}

		/// <summary>
		/// Finds a patient by Id.
		/// </summary>
		[HttpGet("{id}")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetPatient([GuidNotEmpty] Guid id)
		{
			Patient patient = null;

			patient = _patientDbService.FindPatient(id);
			return patient == null ? ReturnProblem("Patient not found.", StatusCodes.Status404NotFound) : Ok(patient);
		}

		/// <summary>
		/// Finds a patient by first name, last name, and date of birth.
		/// </summary>
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
			return patient == null ? ReturnProblem("Patient not found.", StatusCodes.Status404NotFound) : Ok(patient);
        }

		/// <summary>
		/// Adds a new patient to the database.
		/// </summary>
		[HttpPost]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType((StatusCodes.Status415UnsupportedMediaType))]
		public IActionResult CreatePatient(
			[FromBody] Patient patient)
		{
			if (_patientDbService.FindPatient(patient.FirstName,
				patient.LastName, patient.DateOfBirth) != null) {
				return ReturnProblem("Patient already exists.", StatusCodes.Status400BadRequest);
			}

			_patientDbService.AddPatient(patient);

			return NoContent();
		}

		/// <summary>
		/// Updates an existing patient.
		/// </summary>
		[HttpPatch("{id}")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult UpdatePatient(
			[GuidNotEmpty] Guid id,
			[FromBody] PatientDTO patientDTO)
		{
			Patient patient = null;

			patient = _patientDbService.FindPatient(id);
			if (patient == null) {
				return ReturnProblem("Patient not found.", StatusCodes.Status404NotFound);
			}

			patient.Id = id;
			patient.FirstName = patientDTO.FirstName;
			patient.LastName = patientDTO.LastName;
			patient.DateOfBirth = patientDTO.DateOfBirth;
			patient.LastFourOfSSN = patientDTO.LastFourOfSSN;

			_patientDbService.UpdatePatient(patient);

			return NoContent();
		}

		/// <summary>
		/// Deletes an existing patient.
		/// </summary>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeletePatient([GuidNotEmpty] Guid id)
		{
			Patient patient = null;

			patient = _patientDbService.FindPatient(id);
			if (patient == null) {
				return ReturnProblem("Patient not found.", StatusCodes.Status404NotFound);
			}

			_patientDbService.RemovePatient(patient);

			return NoContent();
		}

		/// <summary>
		/// Finds patient contact information by patient Id.
		/// </summary>
		[HttpGet("{id}/contact")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPatientContact([GuidNotEmpty] Guid id)
        {
			PatientContact patientContact = null;

			if (_patientDbService.FindPatient(id) == null) {
				return ReturnProblem("Patient not found.", StatusCodes.Status404NotFound);
			}

			patientContact = _patientDbService.FindPatientContact(id);
			return patientContact == null
				? ReturnProblem("Patient contact not found.", StatusCodes.Status404NotFound)
				: Ok(patientContact);
        }

		/// <summary>
		/// Adds patient contact information  to the database.
		/// </summary>
		[HttpPost("{id}/contact")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult CreatePatientContact(
			[GuidNotEmpty] Guid id,
			[FromBody] PatientContact patientContact)
		{
			if (_patientDbService.FindPatient(id) == null) {
				return ReturnProblem("Patient not found.", StatusCodes.Status404NotFound);
			}

			if (_patientDbService.FindPatientContact(id) != null) {
				return ReturnProblem("Patient contact already exists.", StatusCodes.Status400BadRequest);
			}

			_patientDbService.AddPatientContact(patientContact);

			return NoContent();
		}

		/// <summary>
		/// Updates existing patient contact information.
		/// </summary>
		[HttpPatch("{id}/contact")]
		[Consumes("application/json")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult UpdatePatientContact(
			[GuidNotEmpty] Guid id,
			[FromBody] PatientContactDTO patientContactDTO)
		{
			Patient patient = null;
			PatientContact patientContact = null;

			patient = _patientDbService.FindPatient(id);
			if (patient == null) {
				return ReturnProblem("Patient not found.", StatusCodes.Status404NotFound);
			}

			patientContact = _patientDbService.FindPatientContact(id);
			if (patientContact == null) {
				return ReturnProblem("Patient contact not found.", StatusCodes.Status404NotFound);
			}

			patientContact.PatientId = id;
			patientContact.Patient = patient;
			patientContact.PhoneNumber = patientContactDTO.PhoneNumber;
			patientContact.EmailAddress = patientContactDTO.EmailAddress;

			_patientDbService.UpdatePatientContact(patientContact);

			return NoContent();
		}

		/// <summary>
		/// Deletes existing patient contact information.
		/// </summary>
		[HttpDelete("{id}/contact")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeletePatientContact([GuidNotEmpty] Guid id)
		{
			PatientContact patientContact = null;

			if (_patientDbService.FindPatient(id) == null) {
				return ReturnProblem("Patient not found.", StatusCodes.Status404NotFound);
			}

			patientContact = _patientDbService.FindPatientContact(id);
			if (patientContact == null) {
				return ReturnProblem("Patient contact not found.", StatusCodes.Status404NotFound);
			}

			_patientDbService.RemovePatientContact(patientContact);

			return NoContent();
		}
    }
}
