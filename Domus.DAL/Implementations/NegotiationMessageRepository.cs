using Domus.DAL.Data;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class NegotiationMessageRepository : GenericRepository<NegotiationMessage>, INegotiationMessageRepository
{
	public NegotiationMessageRepository(IAppDbContext dbContext) : base(dbContext)
	{
	}
}
