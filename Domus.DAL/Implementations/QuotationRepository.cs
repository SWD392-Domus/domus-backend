using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class QuotationRepository : GenericRepository<Quotation>, IQuotationRepository
{
	public QuotationRepository(IAppDbContext context) : base(context)
	{
	}
}
