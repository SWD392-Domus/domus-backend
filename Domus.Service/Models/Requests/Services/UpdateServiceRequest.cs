using System.ComponentModel.DataAnnotations;

namespace Domus.Service.Models.Requests.Services;

public class UpdateServiceRequest
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
	[Range(0f, 999999999999f)]
    public double Price { get; set; }

    [Required]
    public string MonetaryUnit { get; set; } = null!;
}
