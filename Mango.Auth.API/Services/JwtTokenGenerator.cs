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
			_jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
		}

		public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
		{
			if (applicationUser == null)
			{
				throw new ArgumentNullException(nameof(applicationUser));
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

			var claims = new List<Claim>
			{
			new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
			new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
			new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName)
			};

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role,role)));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Audience = _jwtOptions.Audience,
				Issuer = _jwtOptions.Issuer,
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
