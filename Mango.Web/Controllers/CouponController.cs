using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponservice;

        public CouponController(ICouponService couponService)
        {        
            _couponservice = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? coupons = new();

            ResponseDTO? response = await _couponservice.GetAllCouponAsync();
            if(response != null && response.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            return View(coupons);
        }

        public async Task<IActionResult> CreateCoupon()
        {

            return View();
        }

        [HttpPost]
		public async Task<IActionResult> CreateCoupon(CouponDto coupon)
		{
            if(ModelState.IsValid)
            {
                ResponseDTO? response = await _couponservice.CreateCouponAsync(coupon);
                if(response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
            }

			return View(coupon);
		}


		public async Task<IActionResult> CouponDelete(int CouponId)
		{
			List<CouponDto>? coupons = new();

			ResponseDTO? response = await _couponservice.GetCouponByIdAsync(CouponId);
			if (response != null && response.IsSuccess)
			{
				CouponDto? model = JsonConvert.DeserializeObject<CouponDto?>(Convert.ToString(response.Result));
                return View(model);
			}
			return NotFound();
		}

        [HttpPost]
		public async Task<IActionResult> CouponDelete(CouponDto CouponDTO)
		{

			ResponseDTO? response = await _couponservice.DeleteCouponAsync(CouponDTO.CouponId);

			if (response != null && response.IsSuccess)
			{
                return RedirectToAction(nameof(CouponIndex));
			}
			return View(CouponDTO);
		}
	}
}
