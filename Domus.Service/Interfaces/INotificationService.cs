using Domus.Common.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;

namespace Domus.Service.Interfaces;

public interface INotificationService : IAutoRegisterable
{
    Task CreateNotification(Notification notification);
    Task<ServiceActionResult> GetNotification(string token);
    Task<ServiceActionResult> UpdateNotificationStatus(string token);

    Task<ServiceActionResult> SearchNotificationsUsingGet(SearchUsingGetRequest request,string token);
    Task<ServiceActionResult> GetPaginatedNotifications(BasePaginatedRequest request);
}