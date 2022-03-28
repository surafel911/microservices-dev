using System;
using System.Text;
using System.Collections.Generic;

using Dapper;

using PatientService.Models;
using DataAtThePointOfCare.Models;

namespace PatientService.Services
{
    public class PatientDbCommandService : IPatientDbCommandService
    {
        public PatientDbCommandService()
        {
        }

        public CommandDefinition GetUUIDModuleCommand() =>
            new CommandDefinition("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");
        
        public CommandDefinition GetCanConnectCommand() =>
            new CommandDefinition(@"SELECT 1;");

        public CommandDefinition GetCreatePatientCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"CREATE TABLE Patients (Id uuid PRIMARY KEY DEFAULT uuid_generate_v4(), ");
            stringBuilder.Append(@"FirstName varchar(40) NOT NULL, LastName varchar(40) NOT NULL, ");
            stringBuilder.Append(@"LastFourOfSSN varchar(4) NOT NULL, DateOfBirth timestamp NOT NULL, RowVersion bytea);");
            
            return new CommandDefinition(stringBuilder.ToString());
        }
            
        
        public CommandDefinition GetAnyPatientCommand() =>
            new CommandDefinition(@"SELECT 1 FROM Patients;");

        public CommandDefinition GetAddPatientCommand(Patient patient)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"INSERT INTO Patient (Id, FirstName, LastName, LastFourOfSSN, DateOfBirth, RowVersion) VALUES (");
            stringBuilder.Append(patient);
            stringBuilder.Append(@");");
            
            return new CommandDefinition(stringBuilder.ToString());
        }

        public CommandDefinition GetAddPatientRangeCommand(IEnumerable<Patient> patients)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (Patient patient in patients) {
                stringBuilder.Append(@"INSERT INTO table_name (Id, FirstName, LastName, LastFourOfSSN, DateOfBirth, RowVersion) VALUES (");
                stringBuilder.Append(patient);
                stringBuilder.Append(@");");
                
                stringBuilder.Append(System.Environment.NewLine);
            }
            
            return new CommandDefinition(stringBuilder.ToString());
        }

        public CommandDefinition GetFindPatientCommand(Guid id)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"SELECT * FROM Patients WHERE Id = ");
            stringBuilder.Append(id);
            stringBuilder.Append(@";");
            
            return new CommandDefinition(stringBuilder.ToString());
        }
            

        public CommandDefinition GetFindPatientCommand(string firstName, string lastName, DateTime dateOfBirth)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"SELECT * FROM Patients WHERE FirstName = ");
            stringBuilder.Append(firstName);
            stringBuilder.Append(@" AND LastName = ");
            stringBuilder.Append(lastName);
            stringBuilder.Append(@" AND DateOfBirth = ");
            stringBuilder.Append(dateOfBirth.Date);
            stringBuilder.Append(";");
            
            return new CommandDefinition(stringBuilder.ToString());
        }


        public CommandDefinition GetUpdatePatientCommand(Patient patient)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"UPDATE Patients SET ");
            stringBuilder.Append(@"FirstName = ");
            stringBuilder.Append(patient.FirstName);
            stringBuilder.Append(@", LastName = ");
            stringBuilder.Append(patient.LastName);
            stringBuilder.Append(@", LastFourOfSSN = ");
            stringBuilder.Append(patient.LastFourOfSSN);
            stringBuilder.Append(@", DateOfBirth = ");
            stringBuilder.Append(patient.DateOfBirth.Date);
            stringBuilder.Append(@" WHERE Id = ");
            stringBuilder.Append(patient.Id);
            stringBuilder.Append(@";");
            
            return new CommandDefinition(stringBuilder.ToString());
        }

        public CommandDefinition GetRemovePatientCommand(Patient patient)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"DELETE FROM Patients WHERE Id = ");
            stringBuilder.Append(patient.Id);
            stringBuilder.Append(@";");
                
            return new CommandDefinition(stringBuilder.ToString());
        }

        public CommandDefinition GetCreatePatientContactCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"CREATE TABLE PatientContacts (PatientId uuid PRIMARY KEY DEFAULT uuid_generate_v4(),");
            stringBuilder.Append(@"PhoneNumber varchar(16) NOT NULL, EmailAddress varchar(40), RowVersion bytea);");
            
            return new CommandDefinition(stringBuilder.ToString());
        }
            

        public CommandDefinition GetAddPatientContactCommand(PatientContact patientContact)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"INSERT INTO Patient (Id, FirstName, LastName, LastFourOfSSN, DateOfBirth, RowVersion) VALUES (");
            stringBuilder.Append(patientContact);
            stringBuilder.Append(@");");
            
            return new CommandDefinition(stringBuilder.ToString());
        }

        public CommandDefinition GetAddPatientContactRangeCommand(IEnumerable<PatientContact> patientContacts)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (PatientContact patientContact in patientContacts) {
                stringBuilder.Append(@"INSERT INTO table_name (Id, FirstName, LastName, LastFourOfSSN, DateOfBirth, RowVersion) VALUES (");
                stringBuilder.Append(patientContact);
                stringBuilder.Append(@");");
                
                stringBuilder.Append(System.Environment.NewLine);
            }
            
            return new CommandDefinition(stringBuilder.ToString());
        }

        public CommandDefinition GetFindPatientContactCommand(Guid patientId)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"SELECT * FROM PatientContacts WHERE PatientId = ");
            stringBuilder.Append(patientId);
            stringBuilder.Append(@";");
            
            return new CommandDefinition(stringBuilder.ToString());
        }

        public CommandDefinition GetUpdatePatientContactCommand(PatientContact patientContact)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"UPDATE PatientContacts SET ");
            stringBuilder.Append(@"PhoneNumber = ");
            stringBuilder.Append(patientContact.PhoneNumber);
            stringBuilder.Append(@", EmailAddress = ");
            stringBuilder.Append(patientContact.EmailAddress);
            stringBuilder.Append(@", RowVersion = ");
            stringBuilder.Append(patientContact.RowVersion);
            stringBuilder.Append(@" WHERE PatientId = ");
            stringBuilder.Append(patientContact.PatientId);
            stringBuilder.Append(@";");
            
            return new CommandDefinition(stringBuilder.ToString());
        }

        public CommandDefinition GetRemovePatientContactCommand(PatientContact patientContact)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"DELETE FROM PatientContacts WHERE PatientId = ");
            stringBuilder.Append(patientContact.PatientId);
            stringBuilder.Append(@";");
                
            return new CommandDefinition(stringBuilder.ToString());
        }
    }
}