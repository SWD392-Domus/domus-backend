using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Email;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;
[Route("/api/[controller]")]
public class EmailController : BaseApiController
{
    private readonly IEmailService _emailService;
    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMail(Email email)
    {
        return await ExecuteServiceLogic(
           async() => await _emailService.SendEmail(email).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
}