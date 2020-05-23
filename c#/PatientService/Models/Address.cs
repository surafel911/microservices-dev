using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PatientService.Models
{
    public class Address
    {
		public PatientContact PatientContact { get; set; }

        [Required]
        public string Street { get ; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public State State { get; set; }

        [Required]
        public string Zip { get; set; }

        public string Details { get; set; }
    }
}
