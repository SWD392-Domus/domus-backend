using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domus.Service.Models.Requests.OfferedPackages;

public class PackageRequest
{
    [Required] 
    public List<Guid> ServiceIds { get;} 
    [Required]
    public List<Guid> ProductDetailIds { get;} 
    public string? Name { get;}
    public double? Discount { get; }
    public List<IFormFile>? Images { get;} = new List<IFormFile>();
}