using Mango.Web.API.Models.DTO;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Identity.Data;

namespace Mango.Web.Service
{
	public class AuthService : IAuthService
	{
		private readonly IBaseService _baseservice;

		public AuthService(IBaseService baseservice)
		{
			_baseservice = baseservice;
		}

		public async Task<ResponseDTO?> CreateRoleAsync(RegistrationRequestDto roleRequest)
		{
			return await _baseservice.SendAsync
			(
			  new RequestDTO()
				  {
					  ApiType = SD.ApiType.POST,
					  Data = roleRequest,
					  Url = SD.AuthApiBase + "/api/auth/AsignRole"

			 });
		}

		public async Task<ResponseDTO?> LoginAsync(LoginRequestDto loginRequest)
		{
			return await _baseservice.SendAsync
			(
			  new RequestDTO()
			  {
				  ApiType = SD.ApiType.POST,
				  Data = loginRequest,
				  Url = SD.AuthApiBase + "/api/auth/login"

			  }
			 );
		}

		public async Task<ResponseDTO?> RegisterASync(RegistrationRequestDto registrationRequest)
		{
			return await _baseservice.SendAsync(
			  new RequestDTO()
			  {
				  ApiType = SD.ApiType.POST,
				  Data = registrationRequest,
				  Url = SD.AuthApiBase + "/api/auth/register"

			  });
		}
	}
}
