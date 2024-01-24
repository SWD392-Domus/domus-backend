using AutoMapper;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Models.Requests;
using Domus.Service.Models.Requests.Articles;
using Domus.Service.Models.Requests.Products;

namespace Domus.Service.AutoMappings;

public static class AutoMapperConfiguration
{
	public static void RegisterMaps(IMapperConfigurationExpression mapper)
	{
		CreateUserMaps(mapper);

		CreateArticleMaps(mapper);
		
		CreateProductMaps(mapper);
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

		mapper.CreateMap<ProductDetail, DtoProductDetail>();
		mapper.CreateMap<CreateProductDetailRequest, ProductDetail>();
	}
}
