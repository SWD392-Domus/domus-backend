using Domus.Api.Controllers.Base;
using Domus.Service.Constants;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;

[Authorize(Roles = UserRoleConstants.ADMIN, AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
public class AdminController : BaseApiController
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard([FromQuery] GetDashboardInfoRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _adminService.GetDashboardInfo(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
}