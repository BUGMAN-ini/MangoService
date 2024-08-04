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
    }
}
