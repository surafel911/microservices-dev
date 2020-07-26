using System;
using System.ComponentModel.DataAnnotations;

namespace PatientService.Models
{
    public class PatientContact
    {
        [Key]
        [Required]
		[GuidNotEmpty]
        public Guid PatientId { get; set; }

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
    }
}
