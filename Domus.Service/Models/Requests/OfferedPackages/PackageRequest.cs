using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domus.Service.Models.Requests.OfferedPackages;

public class PackageRequest
{
    [Required]
    public List<Guid> ServiceIds { get; set; }
    [Required]
    public List<Guid> ProductDetailIds { get; set; }
    public string? Name { get; set; }
    public double? Discount { get; set; }
    public List<IFormFile>? Images { get; set; } = new List<IFormFile>();
}