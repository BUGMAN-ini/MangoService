using AutoMapper;
using Mango.Services.Product.API.ProductMain.Dto;
using Mango.Services.Products.API.Models;
using Mango.Services.Products.API.Models.Dto;

namespace Mango.Services.Coupon.API.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingconfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductMain, ProductDto>().ReverseMap();
                config.CreateMap<PostProductDto, ProductMain>().ReverseMap();
            });
            return mappingconfig;
        }

    }
}
