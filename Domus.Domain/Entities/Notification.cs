using Domus.Domain.Entities.Base;
using Domus.Service.Enums;

namespace Domus.Domain.Entities;

public class Notification : BaseEntity<Guid>
{
    public string RecipientId { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public NotificationStatus Status { get; set; }
    public string? RedirectString { get; set; }

    public string? Image { get; set; }
    public DomusUser Recipient { get; set; }
}