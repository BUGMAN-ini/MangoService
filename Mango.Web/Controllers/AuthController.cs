using Mango.Web.API.Models.DTO;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mango.Web.Controllers
{
	public class AuthController : Controller
	{

		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpGet]
		public async Task<IActionResult> Login()
		{

			LoginRequestDto loginRequestDto = new();
			return View(loginRequestDto);
		}


		[HttpGet]
		public async Task<IActionResult> Register()
		{

			var RoleList = new List<SelectListItem>()
			{
				new SelectListItem
				{
					Text = SD.RoleAdmin,Value = SD.RoleAdmin
				},
				new SelectListItem
				{
					Text = SD.RoleCustomer, Value = SD.RoleCustomer
				}
			};

			ViewBag.RoleList = RoleList;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegistrationRequestDto obj)
		{
			ResponseDTO response = await _authService.RegisterASync(obj);
			ResponseDTO assignRole;
			if (response != null && response.IsSuccess)
			{
				if (string.IsNullOrEmpty(obj.Role))
				{
					obj.Role = SD.RoleCustomer;
				}
				assignRole = await _authService.CreateRoleAsync(obj);
                if (assignRole != null && assignRole.IsSuccess)
				{
					TempData["Success"] = "Registration Succesfull";
					return RedirectToAction(nameof(Login));
				}
            }
            var RoleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = SD.RoleAdmin,Value = SD.RoleAdmin },
                new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };

            ViewBag.RoleList = RoleList;
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            return View();
        }

    }
}
