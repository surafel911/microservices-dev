using System;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataAtThePointOfCare.Models
{
    // TODO: Research how to not have an extra copy of Patient be present when using EF Core ORM
    public class PatientContact
    {
        [Key]
        [Required]
        [GuidNotEmpty]
        public Guid Id { get; set; }
        
        [Required]
		[GuidNotEmpty]
        public Guid PatientId { get; set; }

        // Navagation Property
        [Required]
        public Patient Patient { get; set; }
    
        [Required]
        [Phone]
		[StringLength(16)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
		[StringLength(40)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        
        // Concurrency Check
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(PatientId + ", ");
            stringBuilder.Append(PhoneNumber + ", ");
            stringBuilder.Append(EmailAddress + ", ");
            stringBuilder.Append(RowVersion);

            return stringBuilder.ToString();
        }
    }
}
