using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Packages;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;
[Route("/api/[controller]")]
public class PackagesController : BaseApiController
{
    private readonly IPackageService _packageService;

    public PackagesController(IPackageService packageService)
    {
        _packageService = packageService;
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return await ExecuteServiceLogic(async() => await _packageService.GetAllPackages().ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPackage(Guid id)
    {
        return await ExecuteServiceLogic(async () => await _packageService.GetPackage(id).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePackage(CreatePackageRequest request)
    {
        return await ExecuteServiceLogic(async() => await _packageService.CreatePackage(request).ConfigureAwait(false))
            .ConfigureAwait(false);
    }
    [HttpPost("/test")]
    public async Task<IActionResult> UpdatePackageWithProduct(Guid id, List<Guid> ids)
    {
        return await ExecuteServiceLogic(async() => await _packageService.UpdateWithProduct(id,ids).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    
}

