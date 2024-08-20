using Mango.Auth.API.Models.DTO;
using Mango.Auth.API.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Auth.API.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authservice;
		protected ResponseDto _response;
		public AuthController(IAuthService authservice)
		{
			_authservice = authservice;
			_response = new();
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequest)
		{
			var errorMessage = await _authservice.Register(registrationRequest);
			if(!string.IsNullOrEmpty(errorMessage))
			{
				_response.IsSuccess = false;
				_response.Message = errorMessage;
				return BadRequest(_response);
			}

			return Ok(_response);
		}


		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
		{
			var loginresponse = await _authservice.Login(loginRequest);
			if(loginresponse.User == null)
			{
				_response.IsSuccess = false;
				_response.Message = "Username or password is incorrect";
				return BadRequest(_response);
			}

			_response.Result = loginresponse;
			return Ok(_response);
		}

		[HttpPost("AsignRole")]
		public async Task<IActionResult> AsignRole([FromBody] RegistrationRequestDto model)
		{
			var loginresponse = await _authservice.AssignRole(model.Email,model.Role.ToUpper());
			if (!loginresponse)
			{
				_response.IsSuccess = false;
				_response.Message = "Username or password is incorrect";
				return BadRequest(_response);
			}

			_response.Result = loginresponse;
			return Ok(_response);
		}
	}
}
