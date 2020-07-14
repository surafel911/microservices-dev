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
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    }
}
