using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Products;
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
    [HttpGet("search")]
    public async Task<IActionResult> SearchPackagesUsingGetRequest([FromQuery] SearchUsingGetRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _notificationService.SearchNotificationsUsingGet(request,GetJwtToken()).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [HttpPut]
    public async Task<IActionResult> SeenAllNotification()
    {
        return await ExecuteServiceLogic(async () => await _notificationService.UpdateNotificationStatus(GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }

}