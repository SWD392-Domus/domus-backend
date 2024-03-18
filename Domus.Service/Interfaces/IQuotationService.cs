using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Quotations;

namespace Domus.Service.Interfaces;

public interface IQuotationService : IAutoRegisterable
{
    Task<ServiceActionResult> CreateQuotation(CreateQuotationRequest request, string token);
    Task<ServiceActionResult> DeleteQuotation(Guid id);
    Task<ServiceActionResult> GetAllQuotations();
    Task<ServiceActionResult> GetPaginatedQuotations(BasePaginatedRequest request);
    Task<ServiceActionResult> GetQuotationById(Guid id);
    Task<ServiceActionResult> UpdateQuotation(UpdateQuotationRequest request, Guid id);
    Task<ServiceActionResult> CreateNegotiationMessage(CreateNegotiationMessageRequest request, Guid id);
    Task<ServiceActionResult> GetAllNegotiationMessages(Guid quotatioId);
    Task<ServiceActionResult> GetPaginatedNegotiationMessages(BasePaginatedRequest request, Guid quotationId);
    Task<ServiceActionResult> SearchQuotations(SearchUsingGetRequest request);
    Task<ServiceActionResult> DeleteMultipleQuotations(IEnumerable<Guid> ids);
    Task<ServiceActionResult> GetUserQuotationHistory(string token);
    Task<ServiceActionResult> GetQuotationPriceChangeHistory(Guid quotationId);
    Task<ServiceActionResult> GetQuotationRevisions(Guid id);
    Task<ServiceActionResult> GetQuotationRevision(Guid quotationId, Guid revisionId);
    Task<ServiceActionResult> UpdateQuotationStatus(Guid quotationId, string status);
    Task<ServiceActionResult> SearchUserQuotations(SearchUsingGetRequest request, string getJwtToken);
    Task<ServiceActionResult> SearchStaffQuotations(SearchUsingGetRequest request, string token);
}
