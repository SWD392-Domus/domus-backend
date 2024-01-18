using AutoMapper;
using Domus.Domain.Dtos;
using Domus.Domain.Entities;
using Domus.Service.Models.Requests;

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
	}
}
