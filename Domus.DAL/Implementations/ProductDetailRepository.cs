using Domus.DAL.Data;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ProductDetailRepository : GenericRepository<ProductDetail>, IProductDetailRepository
{
	public ProductDetailRepository(DomusContext context) : base(context)
	{
	}
}
