using System;
using System.ComponentModel.DataAnnotations;

namespace PatientService.Models
{
	[AttributeUsage(
		AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
		AllowMultiple = false)]
	public class GuidNotEmptyAttribute : ValidationAttribute
	{
		public const string DefaultErrorMessage = "The {0} field must not be empty";

		public GuidNotEmptyAttribute() : base(DefaultErrorMessage)
		{
		}

		public override bool IsValid(object value)
		{
			// Guid can never be null.
			if (value is null) {
				return true;
			}

			// If not applied to a Guid, the attribute behaves passively
			switch (value) {
			case Guid guid:
				return guid != Guid.Empty;
			default:
				return true;
			}
		}
	}
}
