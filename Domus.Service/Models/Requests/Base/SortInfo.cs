using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Base;

public class SortInfo
{
    [Required]
    public string FieldName { get; set; } = null!;

    public int Priority { get; set; }
    
    public bool Descending { get; set; }
}