using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;

namespace Domus.Service.Interfaces;

public interface IProductService : IAutoRegisterable
{
    Task<ServiceActionResult> CreateProduct(CreateProductRequest request);
    Task<ServiceActionResult> DeleteProduct(Guid id);
    Task<ServiceActionResult> GetAllProducts();
    Task<ServiceActionResult> GetPaginatedProducts(BasePaginatedRequest request);
    Task<ServiceActionResult> GetProduct(Guid id);
    Task<ServiceActionResult> UpdateProduct(UpdateProductRequest request, Guid id);
    Task<ServiceActionResult> DeleteMultipleProducts(IEnumerable<Guid> ids);
}
