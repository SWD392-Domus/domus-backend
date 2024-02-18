using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.OfferedPackages;
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
        return await ExecuteServiceLogic(async() => 
            await _packageService.GetAllPackages().ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpGet]
    public async Task<IActionResult> GetPaginatedArticles([FromQuery] BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _packageService.GetPaginatedPackages(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPackage(Guid id)
    {
        return await ExecuteServiceLogic(async () => 
            await _packageService.GetPackage(id).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]

    public async Task<IActionResult> CreatePackage([FromForm]PackageRequest request)
    {
        return await ExecuteServiceLogic(async() => 
            await _packageService.CreatePackage(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdatePackage([FromForm]PackageRequest request, Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _packageService.UpdatePackage(request, id).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePackage(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _packageService.DeletePackage(id).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet]
    public async Task<IActionResult> GetPackageByName(string name)
    {
        return await ExecuteServiceLogic(async () =>
            await _packageService.GetPackageByName(name).ConfigureAwait(false)).ConfigureAwait(false);
    }
}

