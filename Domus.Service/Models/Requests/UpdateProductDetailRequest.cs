namespace Domus.Service.Models.Requests;

public class UpdateProductDetailRequest
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Guid ProductId { get; set; }
}
