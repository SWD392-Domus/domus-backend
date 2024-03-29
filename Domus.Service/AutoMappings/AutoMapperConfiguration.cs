using AutoMapper;
using Domus.Domain.Dtos;
using Domus.Domain.Dtos.Articles;
using Domus.Domain.Dtos.Products;
using Domus.Domain.Dtos.Quotations;
using Domus.Domain.Entities;
using Domus.Service.Models.Requests.Articles;
using Domus.Service.Models.Requests.Authentication;
using Domus.Service.Models.Requests.Contracts;
using Domus.Service.Models.Requests.OfferedPackages;
using Domus.Service.Models.Requests.ProductDetails;
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Quotations;
using Domus.Service.Models.Requests.Services;
using Domus.Service.Models.Requests.Users;

namespace Domus.Service.AutoMappings;

public static class AutoMapperConfiguration
{
	public static void RegisterMaps(IMapperConfigurationExpression mapper)
	{
		CreateUserMaps(mapper);

		CreateArticleMaps(mapper);
		
		CreateProductMaps(mapper);

		CreateServiceMaps(mapper);

		CreateQuotationMaps(mapper);

		CreatePackageMaps(mapper);
		
		CreateContractMaps(mapper);

		CreateNotificationMaps(mapper);
	}

	private static void CreateUserMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<RegisterRequest, DomusUser>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom((src) => src.Email))
			.ForMember(dest => dest.FullName, opt => opt.MapFrom((_) => "N/A"));
		mapper.CreateMap<LoginRequest, DomusUser>();
		mapper.CreateMap<DomusUser, DtoDomusUserWithRole>();
		mapper.CreateMap<CreateUserRequest, DomusUser>();
		
		mapper.CreateMap<UpdateUserRequest, DomusUser>()
			.ForMember(dest => dest.Email,
				opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email)))
			.ForMember(dest => dest.UserName,
				opt => opt.Condition(src => !string.IsNullOrEmpty(src.UserName)))
			.ForMember(dest => dest.FullName,
				opt => opt.Condition(src => !string.IsNullOrEmpty(src.FullName)))
			.ForMember(dest => dest.Gender,
				opt => opt.Condition(src => !string.IsNullOrEmpty(src.Gender)))
			.ForMember(dest => dest.Address,
				opt => opt.Condition(src => !string.IsNullOrEmpty(src.Address)))
			.ForMember(dest => dest.PhoneNumber,
				opt => opt.Condition(src => !string.IsNullOrEmpty(src.PhoneNumber)))
			.ForMember(dest => dest.ProfileImage,
				opt => opt.Ignore());
		
		mapper.CreateMap<DomusUser, DtoDomusUser>();
	}

	private static void CreateArticleMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<CreateArticleRequest, Article>();
		mapper.CreateMap<Article, DtoArticle>();
		mapper.CreateMap<Article, DtoArticleWithoutCategory>();

		mapper.CreateMap<ArticleCategory, DtoArticleCategory>();
		mapper.CreateMap<ArticleImage, DtoArticleImage>();

		mapper.CreateMap<UpdateArticleRequest, Article>()
			.ForMember(dest => dest.ArticleImages,
				opt => opt.Ignore())
			.ForMember(dest => dest.ArticleCategoryId,
				opt => opt.Condition((req, _) => req.ArticleCategoryId != default))
			.ForMember(dest => dest.Content,
				opt => opt.Condition((req, _) => !string.IsNullOrEmpty(req.Content)))
			.ForMember(dest => dest.Title,
				opt => opt.Condition((req, _) => !string.IsNullOrEmpty(req.Title)));
	}
	
	private static void CreateProductMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<CreateProductRequest, Product>();
		mapper.CreateMap<CreateProductDetailRequest, ProductDetail>();
		mapper.CreateMap<CreateProductDetailInProductRequest, ProductDetail>();
		mapper.CreateMap<CreateProductPriceRequest, ProductPrice>();
		
		mapper.CreateMap<CreateProductAttributeRequest, ProductAttributeValue>()
			.ForMember(dest => dest.Value,
				opt => opt.MapFrom(src => src.Value))
			.ForMember(dest => dest.ValueType,
				opt => opt.MapFrom(src => src.ValueType))
			.ForPath(dest => dest.ProductAttribute.AttributeName,
				opt => opt.MapFrom(src => src.Name));
		
		mapper.CreateMap<UpdateProductRequest, Product>()
			.ForMember(dest => dest.ProductCategoryId,
				opt => opt.Condition((req, _) => req.ProductCategoryId != default))
			.ForMember(dest => dest.ProductName,
				opt => opt.Condition((req, _) => !string.IsNullOrEmpty(req.ProductName)))
			.ForMember(dest => dest.Brand,
				opt => opt.Condition((req, _) => !string.IsNullOrEmpty(req.Brand)))
			.ForMember(dest => dest.Description,
				opt => opt.Condition((req, _) => !string.IsNullOrEmpty(req.Description)))
			.ForMember(dest => dest.ProductDetails, opt => opt.Ignore());

		mapper.CreateMap<UpdateProductDetailRequest, ProductDetail>()
			.ForMember(dest => dest.DisplayPrice,
				opt => opt.Condition((req, _) => req.DisplayPrice != default));
		
		mapper.CreateMap<UpdateProductAttributeValueRequest, ProductAttributeValue>()
			.ForMember(dest => dest.Value,
				opt => opt.MapFrom(src => src.Value))
			.ForMember(dest => dest.ValueType,
				opt => opt.MapFrom(src => src.ValueType))
			.ForPath(dest => dest.ProductAttribute.AttributeName,
				opt => opt.MapFrom(src => src.Name))
			.ForPath(dest => dest.ProductAttribute.Id,
				opt => opt.MapFrom(src => src.AttributeId));
		
		mapper.CreateMap<UpdateProductImageRequest, ProductImage>()
			.ForMember(dest => dest.ImageUrl,
				opt => opt.Condition((req, _) => !string.IsNullOrEmpty(req.ImageUrl)))
			.ForMember(dest => dest.Width,
				opt => opt.Condition((req, _) => req.Width != default))
			.ForMember(dest => dest.Height,
				opt => opt.Condition((req, _) => req.Height != default));
		
		mapper.CreateMap<UpdateProductPriceRequest, ProductPrice>()
			.ForMember(dest => dest.MonetaryUnit,
				opt => opt.Condition((req, _) => !string.IsNullOrEmpty(req.MonetaryUnit)))
			.ForMember(dest => dest.QuantityType,
				opt => opt.Condition((req, _) => !string.IsNullOrEmpty(req.QuantityType)));
		
		mapper.CreateMap<Product, DtoProduct>()
			.ForMember(dest => dest.ProductDetails,
				opt => opt.MapFrom(src => src.ProductDetails.Where(pd => !pd.IsDeleted)));

		mapper.CreateMap<Product, DtoProductWithoutCategory>()
			.ForMember(dest => dest.ProductDetails,
				opt => opt.MapFrom(src => src.ProductDetails.Where(pd => !pd.IsDeleted)));

		mapper.CreateMap<Product, DtoProductWithoutCategoryAndDetails>();
		mapper.CreateMap<ProductCategory, DtoProductCategory>();
		mapper.CreateMap<ProductDetail, DtoProductDetailPackage>()
			.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));
		
		mapper.CreateMap<ProductDetailQuotationRevision, DtoProductDetailQuotationRevision>()
			.ForMember(dest => dest.ProductName,
				opt => opt.MapFrom(src => src.ProductDetail.Product.ProductName))
			.ForMember(dest => dest.Id,
				opt => opt.MapFrom(src => src.ProductDetailId));
		
		mapper.CreateMap<ProductDetail, DtoProductDetail>()
			.ForMember(dest => dest.DisplayPrice,
				opt => opt.MapFrom(src => Math.Round(src.DisplayPrice, 2)))
			.ForMember(dest => dest.ProductAttributeValues,
				opt => opt.MapFrom((src) => src.ProductAttributeValues.Select(pav => new DtoProductAttributeValue { AttributeId = pav.ProductAttributeId, Name = pav.ProductAttribute.AttributeName, Value = pav.Value, ValueType = pav.ValueType })));
		
		mapper.CreateMap<ProductDetail, DtoProductDetailInStorage>()
			.ForMember(dest => dest.DisplayPrice,
				opt => opt.MapFrom(src => Math.Round(src.DisplayPrice, 2)))
			.ForMember(dest => dest.ProductAttributeValues,
				opt => opt.MapFrom((src) => src.ProductAttributeValues.Select(pav => new DtoProductAttributeValue { AttributeId = pav.ProductAttributeId, Name = pav.ProductAttribute.AttributeName, Value = pav.Value, ValueType = pav.ValueType })))
			.ForMember(dest => dest.ProductName,
				opt => opt.MapFrom(src => src.Product.ProductName))
			.ForMember(dest => dest.Brand,
				opt => opt.MapFrom(src => src.Product.Brand))
			.ForMember(dest => dest.TotalQuantity,
				opt => opt.MapFrom(src => src.ProductPrices.Sum(pp => (int)pp.Quantity)));
		
		mapper.CreateMap<ProductDetail, DtoSingleProductDetail>()
			.ForMember(dest => dest.DisplayPrice,
				opt => opt.MapFrom(src => Math.Round(src.DisplayPrice, 2)))
			.ForMember(dest => dest.ProductName,
				opt => opt.MapFrom(src => src.Product.ProductName))
			.ForMember(dest => dest.Description,
				opt => opt.MapFrom(src => src.Product.Description))
			.ForMember(dest => dest.ProductAttributeValues,
				opt => opt.MapFrom((src) => src.ProductAttributeValues.Select(pav => new DtoProductAttributeValue { AttributeId = pav.ProductAttributeId, Name = pav.ProductAttribute.AttributeName, Value = pav.Value, ValueType = pav.ValueType })));
		
		mapper.CreateMap<ProductImage, DtoProductImage>();
		mapper.CreateMap<ProductPrice, DtoProductPrice>();
		mapper.CreateMap<ProductAttributeValue, DtoProductAttributeValue>();
	}

	private static void CreateServiceMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<Domain.Entities.Service, DtoService>().ReverseMap();
		mapper.CreateMap<CreateServiceRequest,Domain.Entities.Service>();
		mapper.CreateMap<UpdateServiceRequest, Domain.Entities.Service>();
		mapper.CreateMap<ServiceQuotation, DtoServiceQuotation>()
			.ForMember(dest => dest.Name,
				opt => opt.MapFrom(src => src.Service.Name));
	}

	private static void CreateQuotationMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<CreateQuotationRequest, Quotation>()
			.ForMember(dest => dest.ServiceQuotations, opt => opt.Ignore());
		
		mapper.CreateMap<Quotation, DtoQuotation>()
			.ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.UserName))
			.ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.Staff.UserName));
		
		mapper.CreateMap<Quotation, DtoQuotationFullDetails>();
		mapper.CreateMap<QuotationNegotiationLog, DtoQuotationNegotiationLog>();
		mapper.CreateMap<QuotationNegotiationLog, DtoQuotationNegotiationLogWithoutMessages>();
		
		mapper.CreateMap<CreateNegotiationMessageRequest, NegotiationMessage>()
			.ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => DateTime.Now.AddHours(7)));

		mapper.CreateMap<UpdateQuotationRequest, Quotation>()
			.ForMember(dest => dest.CustomerId,
				opt => opt.Condition(src => !string.IsNullOrEmpty(src.CustomerId)))
			.ForMember(dest => dest.StaffId,
				opt => opt.Condition(src => !string.IsNullOrEmpty(src.StaffId)))
			.ForMember(dest => dest.Status,
				opt => opt.Condition(src => !string.IsNullOrEmpty(src.Status)))
			.ForMember(dest => dest.ExpireAt,
				opt => opt.Condition(src => src.ExpireAt != default))
			.ForMember(dest => dest.QuotationRevisions,
				opt => opt.Ignore())
			.ForMember(dest => dest.ServiceQuotations,
				opt => opt.Ignore());
		
		mapper.CreateMap<NegotiationMessage, DtoNegotiationMessage>();
		mapper.CreateMap<QuotationRevision, DtoQuotationRevisionWithPriceAndVersion>();
		mapper.CreateMap<Quotation, DtoQuotationWithoutProductsAndServices>();
		mapper.CreateMap<QuotationRevision, DtoQuotationRevision>();
	}

	private static void CreatePackageMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<PackageRequest,Package>();
		mapper.CreateMap<Package,DtoPackage>();
		mapper.CreateMap<PackageImage,DtoPackageImage>();
		mapper.CreateMap<Package,DtoPackageWithProductName>();
		mapper.CreateMap<Package,DtoPackageProductDetail>();
		mapper.CreateMap<PackageProductDetail, DtoPackageProductDetail>()
			.ForMember(d => d.Id, opt => opt.MapFrom(src => src.ProductDetailId))
			.ForMember(d => d.ProductName, opt => opt.MapFrom(src => src.ProductDetail.Product.ProductName))
			.ForMember(d => d.DisplayPrice, opt => opt.MapFrom(src => src.ProductDetail.DisplayPrice))
			.ForMember(d => d.ProductAttributeValues, opt => opt.MapFrom(src => src.ProductDetail.ProductAttributeValues))
			.ForMember(d => d.ProductImages, opt => opt.MapFrom(src => src.ProductDetail.ProductImages));
	}

	private static void CreateContractMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<ContractRequest, Contract>();
		mapper.CreateMap<Contract, DtoContract>()
			.ForMember(d => d.ServiceQuotations,
				opt => opt.MapFrom(src => src.QuotationRevision.Quotation.ServiceQuotations))
			// .ForMember(d => d.QuotationRevision.ProductDetailQuotationRevisions,opt => opt.MapFrom(src => src.QuotationRevision.ProductDetailQuotationRevisions));
			;
	}
	private static void CreateNotificationMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<DtoNotification, Notification>().ReverseMap();
		
	}
	
}
