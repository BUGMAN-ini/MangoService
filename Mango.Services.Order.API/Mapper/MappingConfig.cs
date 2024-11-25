using AutoMapper;
using Mango.Services.Order.API.Models;
using Mango.Services.Order.API.Models.Dto;

namespace Mango.Services.Order.API.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingconfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDto, CartHeaderDto>().ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal)).ReverseMap();

                config.CreateMap<CartDetailDto, OrderDetailDto>()
                                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

                config.CreateMap<OrderDetailDto, CartDetailDto>().ReverseMap();

                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetailDto, OrderDetails>().ReverseMap();
            });
            return mappingconfig;
        }

    }
}
