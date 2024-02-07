using Domus.Domain.Entities;

namespace Domus.DAL.Interfaces;

public interface IProductDetailRepository : IGenericRepository<ProductDetail>
{
    Task SetModified(ProductDetail productDetail);
}
