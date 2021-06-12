using Auth.Interfaces;
using Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Auth
{
	public class AuthTokenGenerator : IAuthorizationTokenGenerator
	{
		private readonly AuthManager _authManager;

		public AuthTokenGenerator(AuthManager authManager)
		{
			_authManager = authManager;
		}

		public string GenerateToken(UserCredentialsDto credentialsDto)
		{
			return _authManager.GenerateJwt(credentialsDto);
		}
	}
}
