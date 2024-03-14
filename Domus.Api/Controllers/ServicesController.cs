using Domus.Api.Controllers.Base;
using Domus.Service.Constants;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;
[Authorize(Roles = UserRoleConstants.INTERNAL_USER, AuthenticationSchemes = "Bearer")]
[Route("/api/[Controller]")]
public class ServicesController : BaseApiController
{
    private readonly IServiceService _service;

    public ServicesController(IServiceService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetPaginatedArticles([FromQuery] BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _service.GetPaginatedServices(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [AllowAnonymous]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllService()
    {
        return await ExecuteServiceLogic(
            async () => await _service.GetAllServices().ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return await ExecuteServiceLogic(
            async () => await _service.GetService(id).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [HttpPost]
    public async Task<IActionResult> CreateService(CreateServiceRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _service.CreateService(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateService(UpdateServiceRequest request, Guid id)
    {
        return await ExecuteServiceLogic(
            async () => await _service.UpdateService(request,id).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteService(Guid id)
    {
        return await ExecuteServiceLogic(
            async () => await _service.DeleteService(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }
    
    [HttpGet("search")]
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    public async Task<IActionResult> SearchArticlesUsingGetRequest([FromQuery] SearchUsingGetRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _service.SearchServicesUsingGet(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
	
    [HttpDelete("many")]
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    public async Task<IActionResult> DeleteMultipleServices(List<Guid> serviceIds)
    {
        return await ExecuteServiceLogic(
            async () => await _service.DeleteServices(serviceIds).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    
    
}




