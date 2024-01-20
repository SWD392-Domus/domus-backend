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
		#region User maps

		mapper.CreateMap<RegisterRequest, DomusUser>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom((src) => src.Email));
		mapper.CreateMap<LoginRequest, DomusUser>();
		mapper.CreateMap<DomusUser, DtoDomusUser>();
		
		#endregion

		#region Article maps

		mapper.CreateMap<CreateArticleRequest, Article>();
		mapper.CreateMap<Article, DtoArticle>();
		mapper.CreateMap<Article, DtoArticleWithoutCategory>();

		#endregion
		
		#region	Article category maps 

		mapper.CreateMap<ArticleCategory, DtoArticleCategory>();

		#endregion

		#region Article image maps

		mapper.CreateMap<ArticleImage, DtoArticleImage>();

		#endregion

		#region Products maps

		mapper.CreateMap<Product, DtoProduct>();
		mapper.CreateMap<Product, DtoProductWithoutCategoryAndDetails>();
		mapper.CreateMap<CreateProductRequest, Product>();

		#endregion

		#region Product category maps

		mapper.CreateMap<ProductCategory, DtoProductCategory>();

		#endregion

		#region Product detail maps

		mapper.CreateMap<ProductDetail, DtoProductDetail>();

		#endregion
	}
}
