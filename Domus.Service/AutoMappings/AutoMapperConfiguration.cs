using AutoMapper;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Models.Requests.Articles;


using Domus.Service.Models.Requests.Authentication;
using Domus.Service.Models.Requests.ProductDetails;
using Domus.Service.Models.Requests.Products;
using Domus.Service.Models.Requests.Quotations;



namespace Domus.Service.AutoMappings;

public static class AutoMapperConfiguration
{
	public static void RegisterMaps(IMapperConfigurationExpression mapper)
	{
		CreateUserMaps(mapper);

		CreateArticleMaps(mapper);
		
		CreateProductMaps(mapper);

<<<<<<< HEAD
		CreateServiceMaps(mapper);
=======
		CreateQuotationMaps(mapper);
>>>>>>> 4ed70de ([Quotations manamentment][Duy] Remove 'QuotationNegotiationLogId' from 'Quotation' entity)
	}

	private static void CreateUserMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<RegisterRequest, DomusUser>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom((src) => src.Email));
		mapper.CreateMap<LoginRequest, DomusUser>();
		mapper.CreateMap<DomusUser, DtoDomusUser>();
	}

	private static void CreateArticleMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<CreateArticleRequest, Article>();
		mapper.CreateMap<Article, DtoArticle>();
		mapper.CreateMap<Article, DtoArticleWithoutCategory>();

		mapper.CreateMap<ArticleCategory, DtoArticleCategory>();
		mapper.CreateMap<ArticleImage, DtoArticleImage>();
	}
	
	private static void CreateProductMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<Product, DtoProduct>();
		mapper.CreateMap<Product, DtoProductWithoutCategoryAndDetails>();
		mapper.CreateMap<CreateProductRequest, Product>();


		mapper.CreateMap<ProductCategory, DtoProductCategory>();

		mapper.CreateMap<ProductDetail, DtoProductDetail>()
			.ForMember(dest => dest.DisplayPrice, opt => opt.MapFrom((src) => src.DisplayPrice))
			.ForMember(dest => dest.ProductAttributeValues, opt => opt.MapFrom((src) => src.ProductAttributeValues.Select(pav => new DtoProductAttributeValue { Name = pav.ProductAttribute.AttributeName, Value = pav.Value, ValueType = pav.ValueType })))
			.ForMember(dest => dest.ProductName, opt => opt.MapFrom((src) => src.Product.ProductName));
		mapper.CreateMap<CreateProductDetailRequest, ProductDetail>();
	}

<<<<<<< HEAD
	private static void CreateServiceMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<Domain.Entities.Service, DtoService>();
=======
	private static void CreateQuotationMaps(IMapperConfigurationExpression mapper)
	{
		mapper.CreateMap<CreateQuotationRequest, Quotation>()
			.ForMember(dest => dest.Services, opt => opt.Ignore());
		mapper.CreateMap<Quotation, DtoQuotation>();
>>>>>>> 4ed70de ([Quotations manamentment][Duy] Remove 'QuotationNegotiationLogId' from 'Quotation' entity)
	}
}
