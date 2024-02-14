using AutoMapper;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.Packages;

namespace Domus.Service.Implementations;

public class PackageService : IPackageService 
{ 
    private readonly IPackageRepository _packageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductDetailService _productDetailService;
    private readonly IServiceService _serviceService;
    private readonly IMapper _mapper;
    private readonly IPackageImageRepository _packageImageRepository;
    private readonly IProductDetailRepository _productDetailRepository;

    public PackageService(IPackageRepository packageRepository, IPackageImageRepository packageImageRepository, IUnitOfWork unitOfWork,
        IProductDetailService productDetailService, IServiceService serviceService, IMapper mapper, IProductDetailRepository productDetailRepository)
    {
        _packageRepository = packageRepository;
        _packageImageRepository = packageImageRepository;
        _unitOfWork = unitOfWork;
        _productDetailService = productDetailService;
        _serviceService = serviceService;
        _mapper = mapper;
        _productDetailRepository = productDetailRepository;
    }
    
    public async Task<ServiceActionResult> GetAllPackages()
    {
        var packageList = await _packageRepository.FindAsync(x=> x.IsDeleted == false);
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = packageList,
        };
    }

    public async Task<ServiceActionResult> GetPaginatedPackages(BasePaginatedRequest request)
    {
        var packageList = await _packageRepository.FindAsync(x => x.IsDeleted == false);
        var paginatedList = PaginationHelper.BuildPaginatedResult(packageList, request.PageSize, request.PageIndex);
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = paginatedList
        };

    }

    public async Task<ServiceActionResult> GetPackage(Guid packageId)
    {
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = await _packageRepository.FindAsync(x => x.Id == packageId && x.IsDeleted == false)
        };
    }

    public async Task<ServiceActionResult> CreatePackage(CreatePackageRequest createPackageRequest)
    {
 
            var serviceList = await _serviceService.GetServices(createPackageRequest.ServiceIds);
            var productDetailList = await _productDetailService.GetProductDetails(createPackageRequest.ProductDetailIds);
            var package = _mapper.Map<Package>(createPackageRequest);

            package.Services = serviceList.ToList();
            
            await _packageRepository.AddAsync(package);
            await _unitOfWork.CommitAsync();


            package.ProductDetails = productDetailList.ToList();
            await _packageRepository.UpdateAsync(package);
            await _unitOfWork.CommitAsync();
            
            
            return new ServiceActionResult(true);
    }

    

    public Task<ServiceActionResult> UpdatePackage(CreatePackageRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> DeletePackage(Guid packageId)
    {
        throw new NotImplementedException();
    }


}