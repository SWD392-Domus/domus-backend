
using Domus.Domain.Entities;

using Domus.Common.Interfaces;

using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.ProductDetails;
using Domus.Service.Models.Requests.Products;
using Microsoft.AspNetCore.Http;

namespace Domus.Service.Interfaces;

public interface IProductDetailService : IAutoRegisterable
{
    Task<ServiceActionResult> CreateProductDetail(CreateProductDetailRequest request);
    Task<ServiceActionResult> DeleteProductDetail(Guid id);
    Task<ServiceActionResult> GetAllProductDetails();
    Task<ServiceActionResult> GetPaginatedProductDetails(BasePaginatedRequest request);
    Task<ServiceActionResult> GetProductDetailById(Guid id);
    Task<ServiceActionResult> UpdateProductDetail(UpdateProductDetailRequest request, Guid id);
    Task<bool> IsAllProductDetailsExist(IEnumerable<Guid> requestProductDetailIds);
    Task<IQueryable<ProductDetail>> GetProductDetails(List<Guid> productDetailsIds);
    Task<ServiceActionResult> AddImages(IEnumerable<IFormFile> images, Guid id);
    Task<ServiceActionResult> SearchProductDetails(BaseSearchRequest request);
    Task<ServiceActionResult> SearchProductDetailsUsingGet(SearchUsingGetRequest request);
}
