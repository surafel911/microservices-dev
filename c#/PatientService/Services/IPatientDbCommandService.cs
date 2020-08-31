using System;
using System.Collections.Generic;

using Dapper;

using PatientService.Models;
using DataAtThePointOfCare.Models;

namespace PatientService.Services
{
    public interface IPatientDbCommandService
    {
        CommandDefinition GetUUIDModuleCommand();
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
        CommandDefinition GetAddPatientContactCommand(PatientContact patientContact);
        CommandDefinition GetAddPatientContactRangeCommand(IEnumerable<PatientContact> patientContacts);
        CommandDefinition GetFindPatientContactCommand(Guid patientId);
        CommandDefinition GetUpdatePatientContactCommand(PatientContact patientContact);
        CommandDefinition GetRemovePatientContactCommand(PatientContact patientContact);
    }
}