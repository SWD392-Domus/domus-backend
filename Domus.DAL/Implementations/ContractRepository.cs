using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class ContractRepository : GenericRepository<Contract>,IContractRepository
{
    public ContractRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}