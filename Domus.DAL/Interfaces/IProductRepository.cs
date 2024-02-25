using System.Linq.Expressions;
using Domus.Common.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Interfaces;

public interface IProductRepository : IGenericRepository<Product>, IAutoRegisterable
{
    new Task DeleteManyAsync(Expression<Func<Product, bool>> predicate);
}
