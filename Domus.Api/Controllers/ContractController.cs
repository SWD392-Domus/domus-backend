using Domus.Api.Controllers.Base;
using Domus.Service.Interfaces;
using Domus.Service.Models.Requests.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Domus.Api.Controllers;
[Route("/api/[controller]")]
public class ContractController : BaseApiController
{
    private readonly IContractService _contractService;

    public ContractController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return await ExecuteServiceLogic(async () =>
        await _contractService.GetAllContracts().ConfigureAwait(false)).ConfigureAwait(false);
    }
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
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateContract(ContractRequest request, Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.UpdateContract(request,id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteContract( Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.DeleteContract(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpPost("{id:guid}/sign")]
    public async Task<IActionResult> SignContract(string signature, Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _contractService.SignContract(id,signature).ConfigureAwait(false)).ConfigureAwait(false);
    }


}