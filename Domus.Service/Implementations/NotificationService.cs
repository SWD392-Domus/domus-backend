using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
using Domus.Service.Enums;
using Domus.Service.Interfaces;

namespace Domus.Service.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task CreateNotification(Notification notification)
    {
        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IQueryable<Notification>> GetNotificationByClient(string clientId)
    {
        var notifications =  await _notificationRepository.FindAsync(x => x.RecipientId == clientId);
        return notifications;
    }

    public async Task UpdateNotificationStatus(IQueryable<Notification> notifications)
    {
        foreach (var notification in notifications)
        {
            notification.Status = NotificationStatus.Read;
        }
        await _notificationRepository.UpdateManyAsync(notifications);
        await _unitOfWork.CommitAsync();
    }
}