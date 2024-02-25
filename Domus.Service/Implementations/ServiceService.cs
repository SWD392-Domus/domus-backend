﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Articles;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Services;
using Service = Domus.Domain.Entities.Service;
namespace Domus.Service.Implementations;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ServiceService(IServiceRepository serviceRepository,IUnitOfWork unitOfWork, IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceActionResult> GetAllServices()
    {
        var list = (await _serviceRepository.FindAsync(x=> x.IsDeleted == false))
            .ProjectTo<DtoService>(_mapper.ConfigurationProvider).ToList();
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = list
        };
    }

    public async Task<ServiceActionResult> GetPaginatedServices(BasePaginatedRequest request)
    {
        var dtoList = (await _serviceRepository.FindAsync(x=> x.IsDeleted==false)).ProjectTo<DtoService>(_mapper.ConfigurationProvider);
        var paginatedResult = PaginationHelper.BuildPaginatedResult(dtoList, request.PageSize, request.PageIndex);
        return new ServiceActionResult()
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> CreateService(CreateServiceRequest request)
    {
        var service = _mapper.Map<Domain.Entities.Service>(request);
        await _serviceRepository.AddAsync(service);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = service
        };
    }

    public async Task<ServiceActionResult> UpdateService(UpdateServiceRequest request, Guid serviceId)
    {
        var service = await _serviceRepository.GetAsync(x => x.Id == serviceId && x.IsDeleted == false) ?? throw new ServiceNotFoundException();
        _mapper.Map(request,service);
        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> DeleteService(Guid serviceId)
    {
        var service = await _serviceRepository.GetAsync(x => x.Id == serviceId && x.IsDeleted == false) ?? throw new ServiceNotFoundException();
        service.IsDeleted = true;
        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> GetService(Guid serviceId)
    {
        var service = await _serviceRepository.GetAsync(x => x.Id == serviceId && x.IsDeleted == false) ?? throw new ServiceNotFoundException();
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = _mapper.Map<DtoService>(service)
        };
    }

    public async Task<bool> IsAllServicesExist(IEnumerable<Guid> serviceIds)
    {
        foreach (var serviceId in serviceIds)
        {
            var x = await _serviceRepository.GetAsync(x => x.Id == serviceId && x.IsDeleted == false) ??
                    throw new ServiceNotFoundException();
        }
        return true;
    }
    
    // var packageImages = urls.Select(url => new PackageImage() { ImageUrl = url }).ToList();

    // var tasks = serviceIds.Select(async serviceId =>
    // {
    //     var service = await _serviceRepository
    //                       .GetAsync(x => x.Id == serviceId && x.IsDeleted == false)
    //                   ?? throw new ServiceNotFoundException();
    //     return service;
    // });
    // var services = await Task.WhenAll(tasks);
    // return services.AsQueryable();
    // foreach (var serviceId in serviceIds)
    // {
    //     var service = await _serviceRepository.GetAsync(x => x.Id == serviceId && x.IsDeleted == false) ??
    //                   throw new ServiceNotFoundException();
    //     serviceList.Add(service);
    // }
    public async Task<IQueryable<Domain.Entities.Service>> GetServices(IEnumerable<Guid> serviceIds)
    {
        var serviceList = new List<Domain.Entities.Service>();
        foreach (var serviceId in serviceIds)
        {
            var service = await _serviceRepository.GetAsync(x => x.Id == serviceId && x.IsDeleted == false) ??
                          throw new ServiceNotFoundException();
            serviceList.Add(service);
        }
        return serviceList.AsQueryable();
    }
}