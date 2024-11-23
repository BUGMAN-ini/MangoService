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

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartBasedOnUser());
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

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartdto)
        {
            ResponseDTO? response = await cartService.EmailCart(cartdto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email Send Succesfully";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }
    }
}
