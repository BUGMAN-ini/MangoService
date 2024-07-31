using Mango.Web.Models;
using Mango.Web.Service.IService;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseservice;

        public CouponService(IBaseService baseservice)
        {
            _baseservice = baseservice;
        }

        public Task<ResponseDTO?> CreateCouponAsync(CouponDto couponDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> GetAllCouponAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> GetCouponAsync(string couponId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> GetCouponByIdAsync(int couponId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> UpdateCouponAsync(CouponDto couponDto)
        {
            throw new NotImplementedException();
        }
    }
}
