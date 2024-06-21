using System.ComponentModel.DataAnnotations;

namespace Mango.Services.Coupon.API.Models.DTO
{
    public class PostCouponDTO
    {
        public string CouponCode { get; set; }
        public int DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
