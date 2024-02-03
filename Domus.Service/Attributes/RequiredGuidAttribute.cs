using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Attributes;

public class RequiredGuidAttribute : ValidationAttribute
{
	public override bool IsValid(object? value)
	{
		if (value is null)
			return false;
		if (!Guid.TryParse(value.ToString(), out var guidValue))
			return false;
		if (guidValue == default)
			return false;

		return true;
	}
}
