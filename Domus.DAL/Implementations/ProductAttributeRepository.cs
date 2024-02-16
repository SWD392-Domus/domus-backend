using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ProductAttributeRepository : GenericRepository<ProductAttribute>, IProductAttributeRepository
{
	public ProductAttributeRepository(IAppDbContext context) : base(context)
	{
	}
}
