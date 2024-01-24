namespace Domus.Service.Models.Requests.ProductDetails;

public class CreateProductAttributeRequest
{
	public string Name { get; set; }
	public string Value { get; set; }
	public int ValueType { get; set; }
}
