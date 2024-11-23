using IdentityModel;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO>? products = new();

            ResponseDTO? response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(products);
        }

        public async Task<IActionResult> ProductDetails(int ProductId)
        {
            ProductDTO? products = new();

            ResponseDTO? response = await _productService.GetProductByIdAsync(ProductId);
            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            }
            return View(products);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDTO ProductDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                }
            };

            CartDetailDto cartDetails = new CartDetailDto()
            {
                Count = ProductDto.Count,
                ProductId = ProductDto.ProductId,
            };

            List<CartDetailDto> cartDetailsDtos = new() { cartDetails };
            cartDto.CartDetails = cartDetailsDtos;

            ResponseDTO? response = await _productService.UpdateProductAsync(ProductDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item has beed Added to the Shopping Cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(ProductDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
