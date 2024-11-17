using AutoMapper;
using Mango.Services.Products.API.Models;
using Mango.Services.Products.API.Models.Dto;

namespace Mango.Services.Products.API.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingconfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().ReverseMap();
                config.CreateMap<PostProductDto, Product>().ReverseMap();
            });
            return mappingconfig;
        }

    }
}
