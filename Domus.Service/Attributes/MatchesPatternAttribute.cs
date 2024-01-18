using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domus.Service.Attributes;

public class MatchesPatternAttribute : ValidationAttribute
{
    public string Pattern { get; set; }
    public new string ErrorMessage { get; set; } = "Provided value does not match the correct pattern";
    
    public MatchesPatternAttribute(string pattern)
    {
        Pattern = pattern;
    }
    
    public MatchesPatternAttribute(string pattern, string errorMessage) : this(pattern)
    {
        ErrorMessage = errorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var isValid = !string.IsNullOrEmpty(value?.ToString()) && Regex.IsMatch(value.ToString()!, Pattern);
        return isValid ? ValidationResult.Success : new ValidationResult(ErrorMessage);
    }
}