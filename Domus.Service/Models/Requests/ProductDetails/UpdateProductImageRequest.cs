namespace Domus.Service.Models.Requests.ProductDetails;

public class UpdateProductImageRequest
{
    public Guid Id { get; set; }
    
    public string? ImageUrl { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }
}