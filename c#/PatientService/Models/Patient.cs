using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientService.Models
{
	public class Patient
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Required]
		[StringLength(40)]
		[RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
		public string FirstName { get; set; }

		[Required]
		[StringLength(40)]
		[RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
		public string LastName { get; set; }

		[Required]
		[StringLength(4)]
		public string LastFourOfSSN { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime DateOfBirth { get; set; }
	}
}
