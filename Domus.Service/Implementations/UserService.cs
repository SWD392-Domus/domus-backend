using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Users;
using Microsoft.EntityFrameworkCore;

namespace Domus.Service.Implementations;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

    public Task<ServiceActionResult> CreateUser(CreateUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> DeleteUser(string userId)
    {
		var user = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted) ?? throw new UserNotFoundException();
		user.IsDeleted = true;

		await _userRepository.UpdateAsync(user);
		await _unitOfWork.CommitAsync();
		return new ServiceActionResult(true) { Detail = "User deleted successfully" };
    }

    public async Task<ServiceActionResult> GetAllUsers()
    {
		var users = (await _userRepository.GetAllAsync())
			.Where(u => !u.IsDeleted)
			.ProjectTo<DtoDomusUser>(_mapper.ConfigurationProvider);

		return new ServiceActionResult(true) { Data = users };
    }

    public async Task<ServiceActionResult> GetPaginatedUsers(BasePaginatedRequest request)
    {
		var queryableUsers = (await _userRepository.GetAllAsync())
			.Where(u => !u.IsDeleted)
			.ProjectTo<DtoDomusUser>(_mapper.ConfigurationProvider);
		var paginatedResult = PaginationHelper.BuildPaginatedResult(queryableUsers, request.PageSize, request.PageIndex);

		return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public Task<ServiceActionResult> GetUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> UpdateUser(UpdateUserRequest request, string userId)
    {
        throw new NotImplementedException();
    }
}
