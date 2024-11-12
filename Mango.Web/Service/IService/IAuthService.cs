using Mango.Web.API.Models.DTO;
using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
	public interface IAuthService
	{
		Task<ResponseDTO?> LoginAsync(LoginRequestDto loginRequest);
		Task<ResponseDTO?> RegisterAsync(RegistrationRequestDto registrationRequest);
		Task<ResponseDTO?> CreateRoleAsync(RegistrationRequestDto roleRequest);
	}
}
