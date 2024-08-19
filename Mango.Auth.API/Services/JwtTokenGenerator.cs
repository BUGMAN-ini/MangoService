using Mango.Auth.API.Models;
using Mango.Auth.API.Services.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Auth.API.Services
{
	public class JwtTokenGenerator : IJwtToken
	{
		private readonly JwtOptions _jwtOptions;
		public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
		{
			_jwtOptions = jwtOptions.Value;
		}
		public string GenerateToken(ApplicationUser applicationuser)
		{
			var tokenhandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

			var claimlist = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Email,applicationuser.Email),
				new Claim(JwtRegisteredClaimNames.Sub,applicationuser.Id),
				new Claim(JwtRegisteredClaimNames.Name,applicationuser.UserName)
			};

			var tokendescripter = new SecurityTokenDescriptor
			{
				Audience = _jwtOptions.Audience,
				Issuer = _jwtOptions.Issuer,
				Subject = new ClaimsIdentity(claimlist),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

			};

			var token = tokenhandler.CreateToken(tokendescripter);

			return tokenhandler.WriteToken(token);
		}
	}
}
