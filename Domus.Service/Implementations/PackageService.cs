using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.Common.Helpers;
using Domus.DAL.Interfaces;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Base;
using Domus.Service.Models.Requests.OfferedPackages;
using Domus.Service.Models.Requests.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Domus.Service.Implementations;

public class PackageService : IPackageService 
{ 
    private readonly IPackageRepository _packageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductDetailService _productDetailService;
    private readonly IServiceService _serviceService;
    private readonly IPackageProductDetailService _packageProductDetailService;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
   

    public PackageService(IPackageRepository packageRepository, IUnitOfWork unitOfWork,
        IProductDetailService productDetailService, IServiceService serviceService,IPackageProductDetailService packageProductDetailService ,IMapper mapper, IFileService fileService)
    {
        _packageRepository = packageRepository;
        _unitOfWork = unitOfWork;
        _productDetailService = productDetailService;
        _serviceService = serviceService;
        _packageProductDetailService = packageProductDetailService;
        _mapper = mapper;
        _fileService = fileService;
    }
    
    public async Task<ServiceActionResult> GetAllPackages()
    {
        var list = (await _packageRepository.GetAllAsync()).Where(pk => !pk.IsDeleted).ProjectTo<DtoPackageWithProductName>(_mapper.ConfigurationProvider);
        
        foreach (var dtoPackage in list)
        {
            var sumService = dtoPackage.Services.Sum(dtoPackageService => dtoPackageService.Price);
            // var sumProductDetail = dtoPackage.ProductDetails.Sum(dtoPackageProductDetail => dtoPackageProductDetail.DisplayPrice);
            var sumProductDetail = dtoPackage.PackageProductDetails.Sum(x => x.DisplayPrice);
            dtoPackage.EstimatedPrice = (sumService + sumProductDetail) * (100-dtoPackage.Discount)/100;
        }
        
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = list,
        };
    }

    public async Task<ServiceActionResult> GetPaginatedPackages(BasePaginatedRequest request)
    {
        var dtoPackages = (await _packageRepository.GetAllAsync()).Where(pk => !pk.IsDeleted).ProjectTo<DtoPackage>(_mapper.ConfigurationProvider);
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
        var package = (await _packageRepository.FindAsync(x => x.Id == packageId && !x.IsDeleted))
                      .ProjectTo<DtoPackageWithProductName>(_mapper.ConfigurationProvider).FirstOrDefault()
                      ?? throw new PackageNotFoundException();
        var sumService = package.Services.Sum(sv => sv.Price);
        var sumProductDetail = package.PackageProductDetails.Sum(pd => pd.DisplayPrice);
        package.EstimatedPrice = (sumService + sumProductDetail) * (100 - package.Discount) / 100;
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = package
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

            var productDetailsQuantity = packageRequest.ProductDetailIds
                .GroupBy(id => id)
                .Select(group => new PackageProductDetail
                {
                    PackageId = package.Id,
                    ProductDetailId = group.Key,
                    Quantity = group.Count()
                })
                .ToList();
            package.PackageProductDetails = productDetailsQuantity;
            await _packageRepository.UpdateAsync(package);
            await _unitOfWork.CommitAsync();
            return new ServiceActionResult(true);
    }

    

    public async Task<ServiceActionResult> UpdatePackage(PackageRequest request, Guid packageId)
    {
        var package = (await _packageRepository.FindAsync(pk => pk.Id == packageId && pk.IsDeleted == false))
                      .Include(pk => pk.PackageProductDetails)
                      .Include(pk => pk.PackageImages)
                      .Include(pk=> pk.Services)
                      .FirstOrDefault()??
                      throw new PackageNotFoundException();
        package.Name = request.Name ?? package.Name;
        package.Discount = request.Discount ?? package.Discount;
        package.Services = (request.ServiceIds.Count > 0) ? (await _serviceService.GetServices(request.ServiceIds)).ToList() : package.Services ;
        package.PackageProductDetails = (request.ProductDetailIds.Count > 0) ? 
            request.ProductDetailIds
                .GroupBy(id => id)
                .Select(group => new PackageProductDetail
                {
                    PackageId = package.Id,
                    ProductDetailId = group.Key,
                    Quantity = group.Count()
                })
                .ToList() : package.PackageProductDetails;
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

    public async Task<ServiceActionResult> GetPackageByName(string name)
    {
        var packages = await _packageRepository.FindAsync(pk => pk.Name.Contains(name) && !pk.IsDeleted);
        return new ServiceActionResult()
        {
            IsSuccess = true,
            Data = packages.ProjectTo<DtoPackage>(_mapper.ConfigurationProvider)
        };
    }
    public async Task<ServiceActionResult> SearchPackages(BaseSearchRequest request)
    {
        var packages = (await _packageRepository.FindAsync(pk => !pk.IsDeleted)).ToList();
        
        foreach (var searchInfo in request.DisjunctionSearchInfos)
        {
            packages = packages
                .Where(p => ReflectionHelper.GetStringValueByName(typeof(Package), searchInfo.FieldName, p)
                    .Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (request.ConjunctionSearchInfos.Any())
        {
            var initialSearchInfo = request.ConjunctionSearchInfos.First();
            Expression<Func<Package, bool>> conjunctionWhere = p => ReflectionHelper.GetStringValueByName(typeof(Package), initialSearchInfo.FieldName, p)
                .Contains(initialSearchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
            
            foreach (var (searchInfo, i) in request.ConjunctionSearchInfos.Select((value, i) => (value, i)))
            {
                if (i == 0)
                    continue;
                
                Expression<Func<Package, bool>> whereExpr = p => ReflectionHelper.GetStringValueByName(typeof(Package), searchInfo.FieldName, p)
                    .Contains(searchInfo.Keyword, StringComparison.OrdinalIgnoreCase);
                conjunctionWhere = ExpressionHelper.CombineOrExpressions(conjunctionWhere, whereExpr);
            }

            packages = packages.Where(conjunctionWhere.Compile()).ToList();
        }

        if (request.SortInfos.Any())
        {
            request.SortInfos = request.SortInfos.OrderBy(si => si.Priority).ToList();
            var initialSortInfo = request.SortInfos.First();
            Expression<Func<Package, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(Package), initialSortInfo.FieldName, p);

            packages = initialSortInfo.Descending ? packages.OrderByDescending(orderExpr.Compile()).ToList() : packages.OrderBy(orderExpr.Compile()).ToList();
            
            foreach (var (sortInfo, i) in request.SortInfos.Select((value, i) => (value, i)))
            {
                if (i == 0)
                    continue;
                
                orderExpr = p => ReflectionHelper.GetValueByName(typeof(Package), sortInfo.FieldName, p);
                packages = sortInfo.Descending ? packages.OrderByDescending(orderExpr.Compile()).ToList() : packages.OrderBy(orderExpr.Compile()).ToList();
            }
        }

        var paginatedResult = PaginationHelper.BuildPaginatedResult<Package, DtoPackage>(_mapper, packages, request.PageSize, request.PageIndex);

        return new ServiceActionResult(true) { Data = paginatedResult };
    }

    public async Task<ServiceActionResult> SearchPackagesUsingGet(SearchUsingGetRequest request)
    {
        var packages = await (await _packageRepository.FindAsync(p => !p.IsDeleted))
            .ProjectTo<DtoPackage>(_mapper.ConfigurationProvider)
            .ToListAsync();
	    
        if (!string.IsNullOrEmpty(request.SearchField))
        {
            packages = packages
                .Where(p => ReflectionHelper.GetStringValueByName(typeof(DtoPackage), request.SearchField, p).Contains(request.SearchValue ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (!string.IsNullOrEmpty(request.SortField))
        {
            Expression<Func<DtoPackage, object>> orderExpr = p => ReflectionHelper.GetValueByName(typeof(DtoPackage), request.SortField, p);
            packages = request.Descending
                ? packages.OrderByDescending(orderExpr.Compile()).ToList()
                : packages.OrderBy(orderExpr.Compile()).ToList();
        }

        var paginatedResult = PaginationHelper.BuildPaginatedResult(packages, request.PageSize, request.PageIndex);
        var finalProducts = (IEnumerable<DtoPackage>)paginatedResult.Items!;

        paginatedResult.Items = finalProducts;

        return new ServiceActionResult(true) { Data = paginatedResult };
    }

    
    public async Task<ServiceActionResult> DeletePackages(List<Guid> packageIds)
    {
        await _packageRepository.UpdateManyAsync(pk => !pk.IsDeleted && packageIds.Contains(pk.Id));
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true);
    }
}
