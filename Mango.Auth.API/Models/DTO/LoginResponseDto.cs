namespace Mango.Auth.API.Models.DTO
{
	public class LoginResponseDto
	{
		public UserDto User { get; set; }
		public string Token { get; set; }
	}
}
