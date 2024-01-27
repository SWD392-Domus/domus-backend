using Domus.DAL.Data;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ProductDetailQuotationRepository : GenericRepository<ProductDetailQuotation>, IProductDetailQuotationRepository
{
	public ProductDetailQuotationRepository(DomusContext context) : base(context)
	{
	}
}
