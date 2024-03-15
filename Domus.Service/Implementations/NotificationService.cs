using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Enums;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Microsoft.AspNetCore.Identity;

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


    public async Task UpdateNotificationStatus(IQueryable<Notification> notifications)
    {
        foreach (var notification in notifications)
        {
            notification.Status = NotificationStatus.Read;
        }
        await _notificationRepository.UpdateManyAsync(notifications);
        await _unitOfWork.CommitAsync();
    }

    
}