using Domus.DAL.Data;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class QuotationNegotiationLogRepository : GenericRepository<QuotationNegotiationLog>, IQuotationNegotiationLogRepository
{
	public QuotationNegotiationLogRepository(DomusContext context) : base(context)
	{
	}
}
