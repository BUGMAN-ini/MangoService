using System.ComponentModel.DataAnnotations;

namespace Mango.Web.API.Models.DTO
{
	public class RegistrationRequestDto
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string PhoneNum { get; set; }
		[Required]
		public string Password { get; set; }
		public string? Role { get; set; }
	}
}
