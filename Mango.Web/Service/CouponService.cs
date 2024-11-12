using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseservice;

        public  CouponService(IBaseService baseservice)
        {
            _baseservice = baseservice;
        }

        public async Task<ResponseDTO?> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.POST,
                  Data = couponDto,
                  Url = SD.CouponAPIBase + "/api/coupon"

              });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.DELETE,
                  Url = SD.CouponAPIBase + "/api/coupon/" + id

              });
        }

        public async Task<ResponseDTO?> GetAllCouponAsync()
        {
            return await _baseservice.SendAsync(
                new RequestDTO()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.CouponAPIBase+"/api/coupon"
                
                });
        }

        public async Task<ResponseDTO?> GetCouponAsync(string couponId)
        {
            return await _baseservice.SendAsync(
               new RequestDTO()
               {
                   ApiType = SD.ApiType.GET,
                   Url = SD.CouponAPIBase + "/api/coupon/GetByCode"+couponId

               });
        }

        public async Task<ResponseDTO?> GetCouponByIdAsync(int id)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.GET,
                  Url = SD.CouponAPIBase + "/api/coupon/" + id

              });
        }


        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.PUT,
                  Data = couponDto,
                  Url = SD.CouponAPIBase + "/api/coupon"
              });
        }
    }
}
