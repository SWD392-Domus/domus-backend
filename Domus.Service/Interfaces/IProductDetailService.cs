using Domus.Domain.Entities;
using Domus.Service.Models;
using Domus.Service.Models.Requests;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.ProductDetails;

namespace Domus.Service.Interfaces;

public interface IProductDetailService
{
    Task<ServiceActionResult> CreateProductDetail(CreateProductDetailRequest request);
    Task<ServiceActionResult> DeleteProductDetail(Guid id);
    Task<ServiceActionResult> GetAllProductDetails();
    Task<ServiceActionResult> GetPaginatedProductDetails(BasePaginatedRequest request);
    Task<ServiceActionResult> GetProductDetailById(Guid id);
    Task<ServiceActionResult> UpdateProductDetail(UpdateProductDetailRequest request, Guid id);
    Task<bool> IsAllProductDetailsExist(IEnumerable<Guid> requestProductDetailIds);
    Task<IQueryable<ProductDetail>> GetProductDetails(List<Guid> productDetailsIds);
}
