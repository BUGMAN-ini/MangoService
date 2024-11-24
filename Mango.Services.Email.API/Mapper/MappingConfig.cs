using AutoMapper;

namespace Mango.Services.Email.API.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingconfig = new MapperConfiguration(config =>
            {
                //config.CreateMap<Coupons, CouponDTO>().ReverseMap();
                //config.CreateMap<PostCouponDTO, Coupons>().ReverseMap();
            });
            return mappingconfig;
        }

    }
}
