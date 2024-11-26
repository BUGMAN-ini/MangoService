using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDTO?> CreateOrder(CartDto cartDto);
        Task<ResponseDTO?> CreateStripeSession(StripeRequestDto stripeRequest1);
    }
}
