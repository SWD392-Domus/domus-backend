using Domus.Common.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Interfaces;

public interface IPackageRepository : IGenericRepository<Package>, IAutoRegisterable
{
    
}