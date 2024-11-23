using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDTO?> GetCartByUserIdAsync(string userId);
        Task<ResponseDTO?> UpsertCartAsync(CartDto cartdto);
        Task<ResponseDTO?> RemoveCartAsync(int cartDetailsId);
        Task<ResponseDTO?> ApplyCouponAsync(CartDto cart);
        Task<ResponseDTO?> EmailCart(CartDto cartDto);
    }
}
