using AutoMapper;
using Mango.Services.Coupon.API.Models;
using Mango.Services.Coupon.API.Models.DTO;

namespace Mango.Services.Coupon.API.Mapper
{
    public class MappingConfig
    {
            public static MapperConfiguration RegisterMaps()
            {
                var mappingconfig = new MapperConfiguration(config =>
                {
                    config.CreateMap<Coupons, CouponDTO>().ReverseMap();
                    config.CreateMap<PostCouponDTO,Coupons>().ReverseMap();
                });
                return mappingconfig;
            }

    }
}
