using CoursesREST.Data.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoursesREST.Data.Auth
{
	public interface ITokenManager
	{
		Task<string> CreateAcceessTokenAsync(DemoRestUser user);
	}

	public class TokenManager : ITokenManager
	{
		private JwtSecurityToken accessSecurtiyToken;
		private SymmetricSecurityKey _authSignKey;
		private UserManager<DemoRestUser> _userManager;
		private string _issuer;
		private string _audience;

		public TokenManager(IConfiguration configuration, UserManager<DemoRestUser> userManager)
		{
			_authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
			_userManager = userManager;
			_issuer = configuration["JWT:ValidIssuer"];
			_audience = configuration["JWT:ValidAudience"];
		}

		public async Task<string> CreateAcceessTokenAsync(DemoRestUser user)
		{
			var userRoles = await _userManager.GetRolesAsync(user);
			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(CustomClaim.UserId, user.Id.ToString())
			};
			authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

			accessSecurtiyToken = new JwtSecurityToken(
				issuer: _issuer,
				audience: _audience,
				expires: DateTime.UtcNow.AddHours(1),
				claims: authClaims,
				signingCredentials: new SigningCredentials(_authSignKey, SecurityAlgorithms.HmacSha256)
			);

			return new JwtSecurityTokenHandler().WriteToken(accessSecurtiyToken);
		}
	}
}
