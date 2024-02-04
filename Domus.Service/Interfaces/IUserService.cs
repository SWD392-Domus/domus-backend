using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Users;

namespace Domus.Service.Interfaces;

public interface IUserService
{
	Task<ServiceActionResult> GetPaginatedUsers(BasePaginatedRequest request);
	Task<ServiceActionResult> CreateUser(CreateUserRequest request);
	Task<ServiceActionResult> UpdateUser(UpdateUserRequest request, Guid userId);
	Task<ServiceActionResult> GetUser(Guid userId);
	Task<ServiceActionResult> GetAllUsers();
	Task<ServiceActionResult> DeleteUser(Guid userId);
}
