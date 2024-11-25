using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.Order.API.Models.Dto
{
    public class OrderHeaderDto
    {
        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double OrderTotal { get; set; }
        public string? FirstName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? OrderTime { get; set; }
        public string? Status { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? StripsSessionId { get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
