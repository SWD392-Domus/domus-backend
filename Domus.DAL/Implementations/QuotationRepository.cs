using Domus.DAL.Data;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class QuotationRepository : GenericRepository<Quotation>, IQuotationRepository
{
	public QuotationRepository(DomusContext context) : base(context)
	{
	}
}
