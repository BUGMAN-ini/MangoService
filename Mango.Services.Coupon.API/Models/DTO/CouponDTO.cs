using System.ComponentModel.DataAnnotations;

namespace Mango.Services.Coupon.API.Models.DTO
{
    public class CouponDTO
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public int DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
