using Domus.DAL.Data;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ProductDetailRepository : GenericRepository<ProductDetail>, IProductDetailRepository
{
	private readonly DomusContext _dbContext;

	public ProductDetailRepository(DomusContext context) : base(context)
	{
		_dbContext = context;
	}


	public Task SetModified(ProductDetail productDetail)
	{
		_dbContext.SetModified(productDetail);
		return Task.CompletedTask;
	}
}
