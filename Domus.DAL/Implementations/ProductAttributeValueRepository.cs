using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ProductAttributeValueRepository : GenericRepository<ProductAttributeValue>, IProductAttributeValueRepository
{
	public ProductAttributeValueRepository(IAppDbContext context) : base(context)
	{
	}
}
