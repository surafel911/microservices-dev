using System;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientService.Models
{
	public class Patient
	{
		[Key]
		[Required]
		[GuidNotEmpty]
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
		[RegularExpression("^[0-9]+$")]
		public string LastFourOfSSN { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime DateOfBirth { get; set; }
		
		// Concurrency Check
		[Timestamp]
		public byte[] RowVersion { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append(Id + ", ");
			stringBuilder.Append(FirstName + ",");
			stringBuilder.Append(LastName + ",");
			stringBuilder.Append(LastFourOfSSN + ",");
			stringBuilder.Append(DateOfBirth.Date + ",");
			stringBuilder.Append(RowVersion);

			return stringBuilder.ToString();
		}
	}
}
