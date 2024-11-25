using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseservice;

        public OrderService(IBaseService baseservice)
        {
            _baseservice = baseservice;
        }

        public async Task<ResponseDTO?> CreateOrder(CartDto cartDto)
        {
            return await _baseservice.SendAsync(
              new RequestDTO()
              {
                  ApiType = SD.ApiType.POST,
                  Data = cartDto,
                  Url = SD.OrdersAPI + "/api/Orders/CreateOrder"

              });
        }
    }
}
