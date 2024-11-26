using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IOrderService orderService;
        public CartController(ICartService cartService, IOrderService orderService)
        {
            this.cartService = cartService;
            this.orderService = orderService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartBasedOnUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartBasedOnUser());
        }

        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartdto)
        {
            CartDto cart = await LoadCartBasedOnUser();
            cart.CartHeader.Phone = cartdto.CartHeader.Phone;
            cart.CartHeader.Email = cartdto.CartHeader.Email;
            cart.CartHeader.FirstName = cartdto.CartHeader.FirstName;

            var response = await orderService.CreateOrder(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            if(response != null && response.IsSuccess)
            {
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";

                StripeRequestDto striperequest = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation?OrderId=" + orderHeaderDto.OrderHeaderId,
                    CancelUrl = domain + "cart/Checkout",
                    OrderHeader = orderHeaderDto,
                };

                var stripeResponse = await orderService.CreateStripeSession(striperequest);
                StripeRequestDto stripeResponserecieved = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(response.Result));

                Response.Headers.Add("Location", stripeResponserecieved.StripeSessionUrl);
                return new StatusCodeResult(303);
            }
            return View();
        }

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            return View(orderId);
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartdto)
        {
            ResponseDTO? response = await cartService.EmailCart(cartdto);
            cartdto.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email Send Succesfully";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        public async Task<CartDto> LoadCartBasedOnUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDTO? response = await cartService.GetCartByUserIdAsync(userId);
            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }

            return new CartDto();
        }
       
    }
}
