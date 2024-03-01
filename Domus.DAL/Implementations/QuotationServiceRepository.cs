using Domus.DAL.Interfaces;

namespace Domus.DAL.Implementations;

public class QuotationServiceRepository : GenericRepository<Domus.Entities.QuotationService>, IQuotationServiceRepository
{
	public QuotationServiceRepository(IAppDbContext context) : base(context)
	{
	}
}
