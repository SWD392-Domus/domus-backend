using Domus.Common.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Interfaces;

public interface INotificationRepository : IGenericRepository<Notification>, IAutoRegisterable
{
    
}