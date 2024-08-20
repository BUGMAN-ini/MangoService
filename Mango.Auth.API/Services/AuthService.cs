using Mango.Auth.API.Models;
using Mango.Auth.API.Models.DTO;
using Mango.Auth.API.Services.IServices;
using Mango.Services.Auth.API.Data;
using Microsoft.AspNetCore.Identity;

namespace Mango.Auth.API.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _rolemanager;
		private readonly IJwtToken _JwtToken;
		public AuthService(RoleManager<IdentityRole> rolemanager, UserManager<ApplicationUser> userManager, AppDbContext db, IJwtToken jwtToken)
		{
			_rolemanager = rolemanager;
			_userManager = userManager;
			_db = db;
			_JwtToken = jwtToken;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
		{
			var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());

			bool isvalid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
			if (isvalid == false || user is null)
			{
				return new LoginResponseDto() { User = null, Token = "" };
			}
			// if user found generate Token

			var token = _JwtToken.GenerateToken(user);

			UserDto userDto = new()
			{
				Email = user.Email,
				Id = user.Id,
				Name = user.Name,
				PhoneNum = user.PhoneNumber
			};

			LoginResponseDto loginResponseDto = new LoginResponseDto()
			{
				User = userDto,
				Token = token
			};

			return loginResponseDto;
		}

		public async Task<string> Register(RegistrationRequestDto registrationRequest)
		{
			ApplicationUser user = new()
			{
				UserName = registrationRequest.Email,
				Email = registrationRequest.Email,
				NormalizedEmail = registrationRequest.Email.ToUpper(),
				Name = registrationRequest.Name,
				PhoneNumber = registrationRequest.PhoneNum

			};
			try
			{
				var result = await _userManager.CreateAsync(user,registrationRequest.Password);
				if(result.Succeeded)
				{
					var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequest.Email);
					UserDto userDto = new()
					{
						Email = userToReturn.Email,
						Id = userToReturn.Id,
						Name = userToReturn.Name,
						PhoneNum = userToReturn.PhoneNumber
					};

					return "";
				}
				else
				{
					return result.Errors.FirstOrDefault().Description;
				}
			}
			catch (Exception ex)
			{
				return ex.InnerException.ToString();
			}
		}
	}
}
