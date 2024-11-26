using Mango.Services.Order.API.Models.Dto;

namespace Mango.Services.Order.API.Models
{
    public class StripeRequestDto
    {
        public string StripeSessionUrl { get; set; }
        public string StripeSessionId { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancelUrl { get; set; }
        public OrderHeaderDto OrderHeader { get; set; }
    }
}
