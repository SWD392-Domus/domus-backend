using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ProductDetailRepository : GenericRepository<ProductDetail>, IProductDetailRepository
{
	private readonly IAppDbContext _dbContext;

	public ProductDetailRepository(IAppDbContext context) : base(context)
	{
		_dbContext = context;
	}

	public Task SetModified(ProductDetail productDetail)
	{
		_dbContext.SetModified(productDetail);
		return Task.CompletedTask;
	}
}
