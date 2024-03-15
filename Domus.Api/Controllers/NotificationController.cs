using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;
[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]

public class NotificationController : BaseApiController
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    private string GetJwtToken()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
        return authorizationHeader.Remove(authorizationHeader.IndexOf("Bearer", StringComparison.Ordinal), "Bearer".Length).Trim();
    }
    [HttpGet]
    public async Task<IActionResult> GetNotification()
    {
        return await ExecuteServiceLogic(async () => await _notificationService.GetNotification(GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
}