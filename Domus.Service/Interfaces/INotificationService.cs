using Domus.Common.Interfaces;
using Domus.Domain.Entities;
using Domus.Service.Models;

namespace Domus.Service.Interfaces;

public interface INotificationService : IAutoRegisterable
{
    Task CreateNotification(Notification notification);
    Task<ServiceActionResult> GetNotification(string token);

    Task UpdateNotificationStatus(IQueryable<Notification> notifications);
    
}