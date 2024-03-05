using Domus.Common.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Interfaces;

public interface IProductDetailRepository : IGenericRepository<ProductDetail>, IAutoRegisterable
{
    Task SetModified(ProductDetail productDetail);
}
