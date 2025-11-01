using AutoMapper;
using Store.API.Domain.Models;
using Store.API.Domain.Models.Queries;
using Store.API.Resources;

namespace Store.API.Mapping
{
	public class ResourceToModelProfile : Profile
	{
		public ResourceToModelProfile()
		{
			CreateMap<SaveCategoryResource, Category>();
			
			CreateMap<SaveProductResource, Product>()
				.ForMember(src => src.UnitOfMeasurement, opt => opt.MapFrom(src => (UnitOfMeasurement)src.UnitOfMeasurement));
			
			CreateMap<ProductsQueryResource, ProductsQuery>();
		}
	}
}