namespace Domus.Service.Models.Requests.ProductDetails;

public class UpdateProductPriceRequest
{
    public Guid? Id { get; set; }
    
    public double Price { get; set; }

    public string? MonetaryUnit { get; set; }

    public double Quantity { get; set; }

    public string? QuantityType { get; set; }
}