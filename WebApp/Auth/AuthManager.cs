using Auth.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Providers;

namespace WebApp.Auth
{
	public class AuthManager
	{
		private readonly ITimeProvider _timeProvider;
		//TODO: вынеси и сделай норм, черт
		private const string SECRET = "POSOSI";
		private const string ID_CLAIM_TYPE = "ID";
		private const string LOGIN_CLAIM_TYPE = "LOGIN";

		public AuthManager(ITimeProvider timeProvider)
		{
			_timeProvider = timeProvider;
		}

		public string GenerateJwt(UserCredentialsDto credentials)
		{
			var handler = new JwtSecurityTokenHandler();
			//TODO: вынеси куда-нибудь
			var key = Encoding.ASCII.GetBytes(SECRET);

			var claims = GetClaims(credentials);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = _timeProvider.NowUtc(),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = handler.CreateToken(tokenDescriptor);
			return handler.WriteToken(token);
		}

		/// <summary>
		/// Use to get user id and login from generated jwt. Returns null if token was not validated
		/// </summary>
		/// <param name="token">jwt</param>
		/// <returns>Claims info. null if not validated</returns>
		public UserClaimsInfo GetUserInfoByJwt(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(SECRET);
			try
			{
				handler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var id = long.Parse(jwtToken.Claims.FirstOrDefault((c) => c.Type == ID_CLAIM_TYPE).Value);
				var login = jwtToken.Claims.FirstOrDefault((c) => c.Type == LOGIN_CLAIM_TYPE).Value;
				return new UserClaimsInfo(id, login);
			}
			catch
			{
				return null;
			}
		}

		private Claim[] GetClaims(UserCredentialsDto credentials)
		{
			return new Claim[]
			{
				new Claim(ID_CLAIM_TYPE, credentials.UserId.ToString()),
				new Claim(LOGIN_CLAIM_TYPE, credentials.Login)
			};
		}
	}
}
