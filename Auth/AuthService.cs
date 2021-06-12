using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Interfaces;
using Isopoh.Cryptography.Argon2;

namespace Auth.Implementation
{
	public class AuthService : IAuthService
	{
		private readonly ICredentialsRepository _credentialsRepository;
		public AuthService(ICredentialsRepository credentialsRepository)
		{
			_credentialsRepository = credentialsRepository;
		}

		public async Task<AuthResult> Authenticate(string login, string password)
		{
			var credentials = await _credentialsRepository.GetCredentialsAsync(login);
			var authSucceed = Argon2.Verify(credentials.Password, password);
			return new AuthResult
			{
				Succeed = authSucceed,
				Credentials = authSucceed ? null : credentials
			};
		}

		public async Task<AuthResult> Register(string login, string password) 
		{ 
			if (string.IsNullOrEmpty(password) 
				|| string.IsNullOrEmpty(login)
				|| await _credentialsRepository.CheckLoginExistsAsync(login))
			{
				return new AuthResult
				{
					Succeed = false
				};
			}

			var passwordHash = Argon2.Hash(password);
			var credentials = await _credentialsRepository.CreateCredentialsAsync(login, passwordHash);
			return new AuthResult
			{
				Succeed = true,
				Credentials = credentials
			};
		}
	}
}
