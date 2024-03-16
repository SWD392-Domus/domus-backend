using Domus.Service.Enums;

namespace Domus.Domain.Dtos;

public class DtoNotification
{
    public string RecipientId { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public NotificationStatus Status { get; set; }
    public string RedirectString { get; set; }
    public string Image { get; set; }
}