using Microsoft.AspNetCore.Http;

namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductImageRequest
{
	public ICollection<IFormFile> Images { get; set; } = new List<IFormFile>();
}
