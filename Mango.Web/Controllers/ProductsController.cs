using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
