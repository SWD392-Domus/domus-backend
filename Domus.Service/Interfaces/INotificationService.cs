using Domus.Common.Interfaces;
using Domus.Domain.Entities;

namespace Domus.Service.Interfaces;

public interface INotificationService : IAutoRegisterable
{
    Task CreateNotification(Notification notification);
    Task<IQueryable<Notification>> GetNotificationByClient(string clientId);

    Task UpdateNotificationStatus(IQueryable<Notification> notifications);
}