﻿using Domus.Api.Controllers.Base;
using Domus.Service.Constants;
using Domus.Service.Interfaces;
using Domus.Service.Models.Common;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Contracts;
using Domus.Service.Models.Requests.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;
[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ContractController : BaseApiController
{
    private readonly IContractService _contractService;

    public ContractController(IContractService contractService)
    {
        _contractService = contractService;
    }
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return await ExecuteServiceLogic(async () =>
        await _contractService.GetAllContracts().ConfigureAwait(false)).ConfigureAwait(false);
    }
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    [HttpGet("contractor/{id}")]
    public async Task<IActionResult> GetContractorContracts(string id)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.GetContractByContractorId(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpGet("client/{id}")]
    public async Task<IActionResult> GetClientContracts(string id)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.GetContractByClientId(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    [HttpPost]
    public async Task<IActionResult> CreateContract(ContractRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.CreateContract(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetContract(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.GetContract(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateContract(ContractRequest request, Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.UpdateContract(request,id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteContract( Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.DeleteContract(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpPost("{id:guid}/sign")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SignContract([FromForm] FileModels signature, Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.SignContract(id,signature.ImageFile).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpPost("search")]
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    public async Task<IActionResult> SearchContracts([FromForm] BaseSearchRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _contractService.SearchContracts(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    [HttpGet("search")]
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    public async Task<IActionResult> SearchContractsUsingGetRequest([FromQuery] SearchUsingGetRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _contractService.SearchContractsUsingGet(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }
    [HttpGet]
    [Authorize(Roles = UserRoleConstants.INTERNAL_USER)]
    public async Task<IActionResult> GetPaginatedContracts([FromQuery] BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _contractService.GetPaginatedContracts(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

}