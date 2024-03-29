using Domus.Common.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Users;

namespace Domus.Service.Interfaces;

public interface IUserService : IAutoRegisterable
{
	Task<ServiceActionResult> GetPaginatedUsers(BasePaginatedRequest request);
	Task<ServiceActionResult> CreateUser(CreateUserRequest request);
	Task<ServiceActionResult> UpdateUser(UpdateUserRequest request, string userId);
	Task<ServiceActionResult> GetUser(string userId);
	Task<ServiceActionResult> GetAllUsers();
	Task<ServiceActionResult> DeleteUser(string userId);
	Task<ServiceActionResult> GetSelfProfile(string token);
    Task<ServiceActionResult> UpdateSelfProfile(UpdateUserRequest request, string token);
    Task<ServiceActionResult> UpdatePassword(UpdateUserPasswordRequest request, string token);
    Task<ServiceActionResult> GetAllStaff();
    Task<ServiceActionResult> SearchUsersUsingGet(SearchUsingGetRequest request);
    Task<ServiceActionResult> DeleteUsers(List<string> userIds);
}
