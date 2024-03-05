using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domus.Service.Models.Requests.OfferedPackages;

public class PackageRequest
{
    public List<Guid> ServiceIds { get; set; } = new List<Guid>();
    public List<Guid> ProductDetailIds { get; set; } = new List<Guid>();
    public string? Name { get; set; }
    public double? Discount { get; set; }
    public string? Description { get; set; }
    public List<IFormFile>? Images { get; set; } = new List<IFormFile>();
}