using System.ComponentModel.DataAnnotations;

namespace Mango.Services.Coupon.API.Models
{
    public class Coupons
    {
        [Key]
        public int CouponId { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public int DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
