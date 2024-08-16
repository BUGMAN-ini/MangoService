using Microsoft.AspNetCore.Identity;

namespace Mango.Auth.API.Models
{
	public class ApplicationUser : IdentityUser
	{
        public string Name { get; set; }
    }
}
