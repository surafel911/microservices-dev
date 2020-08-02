using System;
using System.Collections.Generic;

using Dapper;

using PatientService.Models;

namespace PatientService.Services
{
    public interface IPatientDbCommandService
    {
        CommandDefinition GetCanConnectCommand();
        CommandDefinition GetCreatePatientCommand();
        CommandDefinition GetAnyPatientCommand();
        CommandDefinition GetAddPatientCommand(Patient patient);
        CommandDefinition GetAddPatientRangeCommand(IEnumerable<Patient> patients);
        CommandDefinition GetFindPatientCommand(Guid id);
        CommandDefinition GetFindPatientCommand(string firstName, string lastName, DateTime dateOfBirth);
        CommandDefinition GetUpdatePatientCommand(Patient patient);
        CommandDefinition GetRemovePatientCommand(Patient patient);
        CommandDefinition GetCreatePatientContactCommand();
        CommandDefinition GetAddPatientContact(PatientContact patientContact);
        CommandDefinition GetAddPatientContactRange(IEnumerable<PatientContact> patientContacts);
        CommandDefinition GetFindPatientContact(Guid patientId);
        CommandDefinition GetUpdatePatientContact(PatientContact patientContact);
        CommandDefinition GetRemovePatientContact(PatientContact patientContact);
    }
}