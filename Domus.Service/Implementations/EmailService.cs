using System.Text.Json.Serialization;
using Domus.Common.Settings;
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
    private readonly EmailSettings _emailSettings;

    public EmailService(IConfiguration config)
    {
        _config = config;
        _emailSettings = _config.GetSection(nameof(EmailSettings)).Get<EmailSettings>() ?? throw new Exception("Invalid SMTP configuration. Check SMTP settings value.");
    }

    
    
    public async Task<ServiceActionResult> SendEmail(BaseEmail request)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailSettings.EmailUsername));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.EmailBody };
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.EmailHost, 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.EmailUsername, _emailSettings.EmailPassword);
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

    public async Task<ServiceActionResult> SendOtpEmail(OtpEmail request)
    {
        return await SendEmail(request);
    }

    public async Task<ServiceActionResult> SendPasswordEmail(PasswordEmail request)
    {
        return await SendEmail(request);
    }
    
    
}