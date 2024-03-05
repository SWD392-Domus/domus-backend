using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class QuotationNegotiationLogRepository : GenericRepository<QuotationNegotiationLog>, IQuotationNegotiationLogRepository
{
	public QuotationNegotiationLogRepository(IAppDbContext context) : base(context)
	{
	}
}
