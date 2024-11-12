using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetAllCouponAsync();
        Task<ResponseDTO?> GetCouponAsync(string couponId);
        Task<ResponseDTO?> GetCouponByIdAsync(int couponId);
        Task<ResponseDTO?> CreateCouponAsync(CouponDto couponDto);
        Task<ResponseDTO?> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDTO?> DeleteCouponAsync(int id);
    }
}
