using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domus.Service.Models.Requests.OfferedPackages;

public class PackageRequest
{
    public List<Guid> ServiceIds { get; set; } 
    public List<Guid> ProductDetgailIds { get; set; } 
    public string? Name { get; set; }
    public double? Discount { get; set; }
    public List<IFormFile>? Images { get; set; } = new List<IFormFile>();
}