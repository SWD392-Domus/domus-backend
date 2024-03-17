using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Enums;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domus.Service.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork
        , IJwtService jwtService, IUserRepository userRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _mapper = mapper;
    }
    public async Task CreateNotification(Notification notification)
    {
        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.CommitAsync();
    }

    public async Task<ServiceActionResult> GetNotification(string token)
    {
        var isValidToken = _jwtService.IsValidToken(token);
        if (!isValidToken)
            throw new InvalidTokenException();
        var userId = _jwtService.GetTokenClaim(token, TokenClaimConstants.SUBJECT)?.ToString() ?? throw new UserNotFoundException();
        var notifications =  (await _notificationRepository.FindAsync(x => x.RecipientId == userId))
            .ProjectTo<DtoNotification>(_mapper.ConfigurationProvider);
        return new ServiceActionResult(true)
        {
            Data = notifications
        };
    }
    
    public async Task<ServiceActionResult> UpdateNotificationStatus(string token)
    {
        var isValidToken = _jwtService.IsValidToken(token);
        if (!isValidToken)
            throw new InvalidTokenException();
        var userId = _jwtService.GetTokenClaim(token, TokenClaimConstants.SUBJECT)?.ToString() ?? throw new UserNotFoundException();
        var notifications = await _notificationRepository.FindAsync(x => x.RecipientId == userId);
            
        foreach (var notification in notifications)
        {
            notification.Status = NotificationStatus.Read;
        }
        await _notificationRepository.UpdateManyAsync(notifications);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> SearchNotificationsUsingGet(SearchUsingGetRequest request,string token)
    {
        var isValidToken = _jwtService.IsValidToken(token);
        if (!isValidToken)
            throw new InvalidTokenException();
        var userId = _jwtService.GetTokenClaim(token, TokenClaimConstants.SUBJECT)?.ToString() ?? throw new UserNotFoundException();
        var notifications = await (await _notificationRepository.FindAsync(x => x.RecipientId == userId))
            .ProjectTo<DtoNotification>(_mapper.ConfigurationProvider)
            .ToListAsync();
        
        
        if (!string.IsNullOrEmpty(request.SearchField))
        {
            notifications = notifications
                .Where(p => ReflectionHelper.GetStringValueByName(typeof(DtoNotification), request.SearchField, p).Contains(request.SearchValue ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (!string.IsNullOrEmpty(request.SortField))
        {
            Expression<Func<DtoNotification, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(DtoNotification), request.SortField, p);
            notifications = request.Descending
                ? notifications.OrderByDescending(orderExpr.Compile()).ToList()
                : notifications.OrderBy(orderExpr.Compile()).ToList();
        }

        var paginatedResult = PaginationHelper.BuildPaginatedResult(notifications, request.PageSize, request.PageIndex);
        var finalProducts = (IEnumerable<DtoNotification>)paginatedResult.Items!;

        paginatedResult.Items = finalProducts;

        return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> GetPaginatedNotifications(BasePaginatedRequest request)
    {
        var dtoNotification = (await _notificationRepository.GetAllAsync()).ProjectTo<DtoPackage>(_mapper.ConfigurationProvider);
        var paginatedList = PaginationHelper.BuildPaginatedResult(dtoNotification, request.PageSize, request.PageIndex);
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = paginatedList
        };
    }
}