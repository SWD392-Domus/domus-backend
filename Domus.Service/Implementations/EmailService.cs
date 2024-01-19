using System.Text.Json.Serialization;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;

namespace Domus.Service.Implementations;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config ;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    
    
    public async Task<ServiceActionResult> SendEmail(Email request)
    {
        var emailConfig = _config.GetSection(nameof(EmailConfig)).Get<EmailConfig>() ?? throw new Exception();
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(emailConfig.EmailUsername));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.EmailBody };
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(emailConfig.EmailHost, 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(emailConfig.EmailUsername, emailConfig.EmailPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> SendSeveralEmail(ServeralEmail serveralEmail)
    {
        foreach (var x in serveralEmail.To)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(x.ToString()));
            email.Subject = serveralEmail.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = serveralEmail.EmailBody };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        return new ServiceActionResult(true);
    }
}