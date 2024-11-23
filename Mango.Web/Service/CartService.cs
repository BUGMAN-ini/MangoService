using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseservice;

        public CartService(IBaseService baseservice)
        {
            _baseservice = baseservice;
        }

        public async Task<ResponseDTO?> ApplyCouponAsync(CartDto cart)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.POST,
                  Data = cart,
                  Url = SD.ShoppingCartAPIBase + "api/Cart/ApplyCoupon"

              });
        }

        public async Task<ResponseDTO?> EmailCart(CartDto cartDto)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.POST,
                  Data = cartDto,
                  Url = SD.ShoppingCartAPIBase + "api/Cart/EmailCartRequest"

              });
        }

        public async Task<ResponseDTO?> GetCartByUserIdAsync(string userId)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.GET,
                  Url = SD.ShoppingCartAPIBase + $"api/Cart/GetCart/{userId}"

              });
        }

        public async Task<ResponseDTO?> RemoveCartAsync(int cartDetailsId)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.POST,
                  Data = cartDetailsId,
                  Url = SD.ShoppingCartAPIBase + $"api/Cart/Remove"

              });
        }
        
        public async Task<ResponseDTO?> UpsertCartAsync(CartDto cartdto)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.POST,
                  Data = cartdto,
                  Url = SD.ShoppingCartAPIBase + $"api/Cart/CartUpsert"

              });
        }
    }
}
