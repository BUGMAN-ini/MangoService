using AutoMapper;
using AzureServiceBus;
using Mango.Services.Order.API.Data;
using Mango.Services.Order.API.Models;
using Mango.Services.Order.API.Models.Dto;
using Mango.Services.Order.API.Services.IServices;
using Mango.Services.Order.API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.Order.API.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly ResponseDTO _response = new();
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        private readonly IProductService _productService;
        private readonly IMessageBus messageBus;
        private readonly IConfiguration _configuration;

        public OrderAPIController(IMapper mapper, AppDbContext db, IProductService productService, IMessageBus messageBus, IConfiguration configuration)
        {
            _mapper = mapper;
            _db = db;
            _productService = productService;
            this.messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpPost("CreateOrder")]
        public async Task<ResponseDTO> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDto>>(cartDto.CartDetails);

                OrderHeader orderheader = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderheader.OrderHeaderId;
                _response.Result = orderheader;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;

            }
            return _response;
        }
    }
}
