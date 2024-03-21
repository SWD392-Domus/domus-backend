using Domus.DAL.Interfaces;
using Domus.Service.Enums;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Dashboard;
using Domus.Service.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Domus.Service.Implementations;

public class AdminService : IAdminService
{
	private readonly IContractRepository _contractRepository;
	private readonly IQuotationRevisionRepository _quotationRevisionRepository;
	private readonly IQuotationRepository _quotationRepository;
	private readonly IUserRepository _userRepository;

	public AdminService(IContractRepository contractRepository, IQuotationRevisionRepository quotationRevisionRepository, IUserRepository userRepository, IQuotationRepository quotationRepository)
	{
		_contractRepository = contractRepository;
		_quotationRevisionRepository = quotationRevisionRepository;
		_userRepository = userRepository;
		_quotationRepository = quotationRepository;
	}

    public async Task<ServiceActionResult> GetDashboardInfo(GetDashboardInfoRequest request)
    {
	    var contracts = await (await _contractRepository.FindAsync(c =>
			    !c.IsDeleted
			    && c.Status == ContractStatus.SIGNED
			    && c.SignedAt.HasValue
			    && c.SignedAt.Value.Year == request.Year))
		    .ToListAsync();

        var dashboardResponse = new DashboardResponse();

		for (var i = 1; i <= 12; i++)
		{
			var contractsByMonth = contracts.Where(c => c.SignedAt.HasValue && c.SignedAt.Value.Month == i);
			var revenueByMonth = await (await _quotationRevisionRepository.FindAsync(qr => 
					contractsByMonth.Select(c => c.QuotationRevisionId).Contains(qr.Id)))
				.Select(qr => qr.TotalPrice)
				.SumAsync();

			dashboardResponse.RevenueByMonths.Add(new()
			{
				Revenue = (float)revenueByMonth,
				MonthAsNumber = i,
				MonthAsString = GetMonthAsString(i)
			});
		}

		dashboardResponse.ContractsCount = contracts.Count;
		dashboardResponse.QuotationsCount = await (await _quotationRepository.FindAsync(q => !q.IsDeleted && q.CreatedAt.Year == request.Year)).CountAsync();
		dashboardResponse.NewUsersCount = await (await _userRepository.FindAsync(u => !u.IsDeleted && u.EmailConfirmed)).CountAsync();
		dashboardResponse.TotalRevenue = dashboardResponse.RevenueByMonths.Select(rbm => rbm.Revenue).Sum();

        return new ServiceActionResult(true) { Data = dashboardResponse };
    }

	private string GetMonthAsString(int month)
	{
		return month switch
		{
			1 => "January",
			2 => "February",
			3 => "March",
			4 => "April",
			5 => "May",
			6 => "June",
			7 => "July",
			8 => "August",
			9 => "September",
			10 => "October",
			11 => "November",
			12 => "December",
			_ => throw new ArgumentOutOfRangeException(nameof(month), month, null)
		};
	}
}
