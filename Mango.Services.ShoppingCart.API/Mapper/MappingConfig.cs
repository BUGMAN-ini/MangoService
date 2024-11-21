using AutoMapper;
using Mango.Services.ShoppingCart.API.Models;
using Mango.Services.ShoppingCart.API.Models.Dto;

namespace Mango.Services.ShoppingCart.API.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingconfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailDto>().ReverseMap();
            });
            return mappingconfig;
        }

    }
}
