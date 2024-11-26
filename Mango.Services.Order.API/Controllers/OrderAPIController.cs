using AutoMapper;
using AzureServiceBus;
using Mango.Services.Order.API.Data;
using Mango.Services.Order.API.Models;
using Mango.Services.Order.API.Models.Dto;
using Mango.Services.Order.API.Services.IServices;
using Mango.Services.Order.API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

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

        [HttpPost("CreateStripeSession")]
        [Authorize]
        public async Task<ResponseDTO> CreateStripeSession([FromBody] StripeRequestDto stripeRequest)
        {
            try
            {
                    var options = new Stripe.Checkout.SessionCreateOptions
                    {
                        SuccessUrl = stripeRequest.ApprovedUrl,
                        CancelUrl = stripeRequest.CancelUrl,
                        LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                        {
                            new Stripe.Checkout.SessionLineItemOptions
                            {
                                Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
                                Quantity = 2,
                            },
                        },
                        Mode = "payment",
                    };

                foreach(var item in stripeRequest.OrderHeader.OrderDetails)
                {
                    var sessionlineitem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionlineitem);
                }

                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);
                stripeRequest.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == stripeRequest.OrderHeader.OrderHeaderId);
                orderHeader.StripsSessionId = session.Id;
                _db.SaveChanges();
                _response.Result = stripeRequest;
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
