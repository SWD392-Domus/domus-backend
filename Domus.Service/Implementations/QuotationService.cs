using Domus.DAL.Interfaces;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Quotations;

namespace Domus.Service.Implementations;

public class QuotationService : IQuotationService
{
	private readonly IQuotationRepository _quotationRepository;
	private readonly IUnitOfWork _unitOfWork;

	public QuotationService(IQuotationRepository quotationRepository, IUnitOfWork unitOfWork)
	{
		_quotationRepository = quotationRepository;
		_unitOfWork = unitOfWork;
	}

    public Task<ServiceActionResult> CreateQuotation(CreateQuotationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> DeleteQuotation(Guid id)
    {
		var quotation = await _quotationRepository.GetAsync(q => q.Id == id);
		if (quotation == null)
			throw new QuotationNotFoundException();
		
		quotation.IsDeleted = true;
		await _quotationRepository.UpdateAsync(quotation);
		await _unitOfWork.CommitAsync();

		return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> GetAllQuotations()
    {
		await Task.CompletedTask;
		return new ServiceActionResult(true);
    }

    public Task<ServiceActionResult> GetPaginatedQuotations(BasePaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> GetQuotationById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> UpdateQuotation(UpdateQuotationRequest request, Guid id)
    {
        throw new NotImplementedException();
    }
}
