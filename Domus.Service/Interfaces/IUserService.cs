using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Users;

namespace Domus.Service.Interfaces;

public interface IUserService
{
	Task<ServiceActionResult> GetPaginatedUsers(BasePaginatedRequest request);
	Task<ServiceActionResult> CreateUser(CreateUserRequest request);
	Task<ServiceActionResult> UpdateUser(UpdateUserRequest request, string userId);
	Task<ServiceActionResult> GetUser(string userId);
	Task<ServiceActionResult> GetAllUsers();
	Task<ServiceActionResult> DeleteUser(string userId);
}
