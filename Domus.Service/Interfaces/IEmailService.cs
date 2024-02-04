using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Email;

namespace Domus.Service.Interfaces;

public interface IEmailService : IAutoRegisterable
{
    Task<ServiceActionResult> SendEmail(BaseEmail baseEmail);
    Task<ServiceActionResult> SendSeveralEmail(ServeralEmail email);
}
