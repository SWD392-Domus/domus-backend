using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ServiceQuotationRepository : GenericRepository<ServiceQuotation>, IQuotationServiceRepository
{
	public ServiceQuotationRepository(IAppDbContext context) : base(context)
	{
	}
}
