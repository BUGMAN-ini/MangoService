using Mango.Auth.API.Models.DTO;

namespace Mango.Auth.API.Services.IServices
{
	public interface IAuthService
	{
		Task<string> Register(RegistrationRequestDto registrationRequest);
		Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
		Task<bool> AssignRole(string email, string rolename);

	}
}
