using Domus.Common.Interfaces;
using Domus.Domain.Entities;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Contracts;
using Domus.Service.Models.Requests.OfferedPackages;
using Domus.Service.Models.Requests.Products;

namespace Domus.Service.Interfaces;

public interface IContractService : IAutoRegisterable
{
    Task<ServiceActionResult> GetAllContracts();
    Task<ServiceActionResult> GetPaginatedContracts(BasePaginatedRequest request);
    Task<ServiceActionResult> GetContract(Guid ContractId);
    Task<ServiceActionResult> CreateContract(ContractRequest request);
    Task<ServiceActionResult> UpdateContract(ContractRequest request, Guid ContractId);
    Task<ServiceActionResult> DeleteContract(Guid ContractId);
    Task<ServiceActionResult> GetContractByClientId(string clientId);
    Task<ServiceActionResult> GetContractByContractorId(string contractorId);
    Task<ServiceActionResult> SearchContracts(BaseSearchRequest request);
    Task<ServiceActionResult> SearchContractsUsingGet(SearchUsingGetRequest request);
    Task<ServiceActionResult> DeleteContracts(List<Guid> ContractIds);

    Task<ServiceActionResult> SignContract(Guid contractId, string signature);
}