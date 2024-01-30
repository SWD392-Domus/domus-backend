using Domus.Service.Models;
using Domus.Service.Models.Email;

namespace Domus.Service.Interfaces;

public interface IEmailService
{
    Task<ServiceActionResult> SendEmail(Email email);
    Task<ServiceActionResult> SendSeveralEmail(ServeralEmail email);

    Task<ServiceActionResult> SendOtpEmail(OtpEmail otpEmail);
}