using Domus.DAL.Interfaces;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;

namespace Domus.Service.Implementations;

public class ProductService : IProductService
{
	private IProductRepository _productRepository;
	
	public ProductService(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

    public Task<ServiceActionResult> CreateProduct(CreateProductRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> DeleteProduct(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> GetAllProducts()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> GetPaginatedProducts(BasePaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> GetProduct(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> UpdateProduct(UpdateProductRequest request, Guid id)
    {
        throw new NotImplementedException();
    }
}
