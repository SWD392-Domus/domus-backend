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
            foreach (var productDetail in productDetailList)
            {
                // productDetail.Packages.Add(package); 
                productDetail.Id = new Guid();
                await _productDetailRepository.UpdateAsync(productDetail);
                package.ProductDetails.Add(productDetail);
                // await _productDetailRepository.SetModified(productDetail);
            }
            foreach (var service in serviceList)
            {
                package.Services.Add(service);
            }
            
            await _packageRepository.AddAsync(package);
            await _unitOfWork.CommitAsync();
            return new ServiceActionResult(true);
    }

    private async void UpdatePackageFullOption(Guid id,List<Guid> serviceIds, List<Guid> productDetailIds)
    {
        var serviceList = await _serviceService.GetServices(serviceIds);
        // var productDetailList = await _productDetailService.GetProductDetails(productDetailIds);
        var fullPackage = await _packageRepository.GetAsync(x => x.Id == id) ??
                          throw new Exception("PACKAGE NOT FOUND");
        // var packageImages = new List<PackageImage>
        // {
        //     new PackageImage {ImageUrl = "Image URL 1", Width = 100, Height = 200 },
        //     new PackageImage { ImageUrl = "Image URL 2", Width = 150, Height = 250 }
        // };
        // foreach (var productDetail in productDetailList)
        // {
        //     fullPackage.ProductDetails.Add(productDetail);
        // }
        foreach (var service in serviceList)
        {
            fullPackage.Services.Add(service);
        }
        // foreach (var packageImage in packageImages)
        // {
        //     fullPackage.PackageImages.Add(packageImage);
        // }
        await _packageRepository.UpdateAsync(fullPackage);
        await _unitOfWork.CommitAsync();
        Console.WriteLine("SUCCESS UPDATE");
    }

    public Task<ServiceActionResult> UpdatePackage(CreatePackageRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceActionResult> DeletePackage(Guid packageId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> UpdateWithProduct(Guid id,List<Guid> ids)
    {
        var x = await _packageRepository.GetAsync(x => x.Id == id);
        var listProductDetail = await _productDetailService.GetProductDetails(ids);
        x.ProductDetails = listProductDetail.ToList();
        await _packageRepository.UpdateAsync(x);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }
}