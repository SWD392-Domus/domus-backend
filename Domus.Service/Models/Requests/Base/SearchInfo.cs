using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Base;

public class SearchInfo
{
    [Required]
    public string FieldName { get; set; } = null!;

    [Required]
    public string Keyword { get; set; } = null!;
}