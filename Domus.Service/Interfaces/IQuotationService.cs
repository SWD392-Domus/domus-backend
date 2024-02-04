using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Quotations;

namespace Domus.Service.Interfaces;

public interface IQuotationService : IAutoRegisterable
{
    Task<ServiceActionResult> CreateQuotation(CreateQuotationRequest request);
    Task<ServiceActionResult> DeleteQuotation(Guid id);
    Task<ServiceActionResult> GetAllQuotations();
    Task<ServiceActionResult> GetPaginatedQuotations(BasePaginatedRequest request);
    Task<ServiceActionResult> GetQuotationById(Guid id);
    Task<ServiceActionResult> UpdateQuotation(UpdateQuotationRequest request, Guid id);
    Task<ServiceActionResult> CreateNegotiationMessage(CreateNegotiationMessageRequest request, Guid id);
    Task<ServiceActionResult> GetAllNegotiationMessages(Guid quotatioId);
    Task<ServiceActionResult> GetPaginatedNegotiationMessages(BasePaginatedRequest request, Guid quotationId);
}
