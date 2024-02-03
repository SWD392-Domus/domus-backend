namespace Domus.Service.Models.Requests.ProductDetails;

public class UpdateProductDetailRequest
{
	public string? Name { get; set; }
	public string? Description { get; set; }
	public Guid ProductId { get; set; }
}
