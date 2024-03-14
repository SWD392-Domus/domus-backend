using Domus.DAL.Interfaces;
using Domus.Domain.Entities;

namespace Domus.DAL.Implementations;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}