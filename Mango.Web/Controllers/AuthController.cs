﻿using Mango.Web.API.Models.DTO;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
	public class AuthController : Controller
	{

		private readonly IAuthService _authService;
		private readonly ITokerProvider _provider;

        public AuthController(IAuthService authService, ITokerProvider provider)
        {
            _authService = authService;
            _provider = provider;
        }

        [HttpGet]
		public async Task<IActionResult> Login()
		{

			LoginRequestDto loginRequestDto = new();
			return View(loginRequestDto);
		}


		[HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {

            ResponseDTO response = await _authService.LoginAsync(obj);
            if (response != null && response.IsSuccess)
            {
				LoginResponseDto loginresponse = 
					JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

				_provider.SetToken(loginresponse.Token);
				return RedirectToAction("Index", "Home");
            }
			else
			{
				ModelState.AddModelError("CustomError", response.Message);
				return View(obj);
			}

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

		private async Task SignInUser(LoginResponseDto login)
		{
			var handler = new JwtSecurityTokenHandler();

			var jwt = handler.ReadJwtToken(login.Token);

			var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
			identity.AddClaim();



			var principal = new ClaimsPrincipal();
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,)
		}

    }
}
