using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ServiceRepository : GenericRepository<Domain.Entities.Service>, IServiceRepository
{
    public ServiceRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}