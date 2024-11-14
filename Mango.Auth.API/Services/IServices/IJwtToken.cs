using Mango.Auth.API.Models;

namespace Mango.Auth.API.Services.IServices
{
	public interface IJwtToken
	{
		string GenerateToken(ApplicationUser applicationuser, IEnumerable<string> roles);
	}
}

