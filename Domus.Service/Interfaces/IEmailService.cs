using Domus.Service.Models;
using Domus.Service.Models.Email;

namespace Domus.Service.Interfaces;

public interface IEmailService
{
    Task<ServiceActionResult> SendEmail(BaseEmail baseEmail);
    Task<ServiceActionResult> SendSeveralEmail(ServeralEmail email);

   
}