using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.OfferedPackages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Domus.Service.Implementations;

public class PackageService : IPackageService 
{ 
    private readonly IPackageRepository _packageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductDetailService _productDetailService;
    private readonly IServiceService _serviceService;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
   

    public PackageService(IPackageRepository packageRepository, IUnitOfWork unitOfWork,
        IProductDetailService productDetailService, IServiceService serviceService, IMapper mapper, IFileService fileService)
    {
        _packageRepository = packageRepository;
        _unitOfWork = unitOfWork;
        _productDetailService = productDetailService;
        _serviceService = serviceService;
        _mapper = mapper;
        _fileService = fileService;
    }
    
    public async Task<ServiceActionResult> GetAllPackages()
    {
        var packageList = (await _packageRepository.FindAsync(x=> x.IsDeleted == false))
            .Include(pk => pk.Services)
            .Include(pk => pk.ProductDetails)
                .ThenInclude(pd=> pd.ProductAttributeValues)
            .Include(pk => pk.ProductDetails)
                .ThenInclude(pd => pd.ProductImages)
            .Include(pk => pk.PackageImages);
        var dtoPackages = new List<DtoPackage>();
        foreach (var pk in packageList)
        {
            dtoPackages.Add(_mapper.Map<DtoPackage>(pk));
        }
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = dtoPackages,
        };
    }

    public async Task<ServiceActionResult> GetPaginatedPackages(BasePaginatedRequest request)
    {
        var packageList = (await _packageRepository.FindAsync(x=> x.IsDeleted == false))
            .Include(pk => pk.Services)
            .Include(pk => pk.ProductDetails)
            .ThenInclude(pd=> pd.ProductAttributeValues)
            .Include(pk => pk.ProductDetails)
            .ThenInclude(pd => pd.ProductImages)
            .Include(pk => pk.PackageImages);
        var dtoPackages = new List<DtoPackage>();
        foreach (var pk in packageList)
        {
            dtoPackages.Add(_mapper.Map<DtoPackage>(pk));
        }
        var paginatedList = PaginationHelper.BuildPaginatedResult(dtoPackages.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = paginatedList
        };
    }

    [SuppressMessage("ReSharper.DPA", "DPA0009: High execution time of DB command", MessageId = "time: 627ms")]
    public async Task<ServiceActionResult> GetPackage(Guid packageId)
    {
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data =_mapper.Map<DtoPackage>((await _packageRepository.FindAsync(x=> x.Id == packageId && !x.IsDeleted))
                      .Include(pk => pk.Services)
                      .Include(pk => pk.ProductDetails)
                      .ThenInclude(pd=> pd.ProductAttributeValues)
                      .Include(pk => pk.ProductDetails)
                      .ThenInclude(pd => pd.ProductImages)
                      .Include(pk => pk.PackageImages)
                      .FirstOrDefault()
                      )
                 ?? throw new PackageNotFoundException()
        };
    }

    public async Task<ServiceActionResult> CreatePackage(PackageRequest packageRequest)
    {
            var package = _mapper.Map<Package>(packageRequest);
            package.Services =  (await _serviceService.GetServices(packageRequest.ServiceIds)).ToList();
            await _packageRepository.AddAsync(package);
            await _unitOfWork.CommitAsync();
            if (packageRequest.Images?.Count > 0)
            {
                var urls = await _fileService.GetUrlAfterUploadedFile(packageRequest.Images);
                var packageImages = urls.Select(url => new PackageImage() { ImageUrl = url }).ToList();
                package.PackageImages = packageImages;
            }
            package.ProductDetails = (await _productDetailService.GetProductDetails(packageRequest.ProductDetailIds)).ToList();
            await _packageRepository.UpdateAsync(package);
            await _unitOfWork.CommitAsync();
            return new ServiceActionResult(true);
    }

    

    public async Task<ServiceActionResult> UpdatePackage(PackageRequest request, Guid packageId)
    {
        var package = await _packageRepository.GetAsync(pk => pk.Id == packageId && pk.IsDeleted == false) ??
                      throw new PackageNotFoundException();
        package.Name = request.Name ?? package.Name;
        package.Discount = request.Discount ?? package.Discount;
        package.Services = (request.ServiceIds.Count > 0) ? (await _serviceService.GetServices(request.ServiceIds)).ToList() : package.Services ;
        package.ProductDetails = (request.ProductDetailIds.Count > 0) ? 
            (await _productDetailService.GetProductDetails(request.ProductDetailIds)).ToList() : package.ProductDetails;
        if (request.Images?.Count > 0)
        {
            var urls = await _fileService.GetUrlAfterUploadedFile(request.Images);
            var packageImages = urls.Select(url => new PackageImage() { ImageUrl = url }).ToList();
            package.PackageImages = packageImages;
        }
        await _packageRepository.UpdateAsync(package);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }

    public async Task<ServiceActionResult> DeletePackage(Guid packageId)
    {
        var package = await _packageRepository.GetAsync(pk => pk.Id == packageId && pk.IsDeleted == false) ??
                      throw new PackageNotFoundException();
        package.IsDeleted = true;
        await _packageRepository.UpdateAsync(package);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }


}