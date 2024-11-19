using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO>? products = new();

            ResponseDTO? response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(products);
        }

		public async Task<IActionResult> CreateProduct()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateProduct(ProductDTO product)
		{
			if (ModelState.IsValid)
			{
				ResponseDTO? response = await _productService.CreateProductAsync(product);
				if (response != null && response.IsSuccess)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}

			return View(product);
		}


        public async Task<IActionResult> ProductDelete(int CouponId)
        {
            List<ProductDTO>? coupons = new();

            ResponseDTO? response = await _productService.GetProductAsync(CouponId);
            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO?>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDTO CouponDTO)
        {

            ResponseDTO? response = await _productService.DeleteProductAsync(CouponDTO.ProductId);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
            return View(CouponDTO);
        }
    }
}
