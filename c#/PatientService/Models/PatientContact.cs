using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PatientService.Models
{
    public class PatientContact
    {
        [Key]
        [Required]
        public Guid PatientId { get; set; }

        [Required]
        public Patient Patient { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public Address Address { get; set; }
    }
}
