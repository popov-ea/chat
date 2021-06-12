using Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Interfaces
{
	public interface IAuthorizationTokenGenerator
	{
		public string GenerateToken(UserCredentialsDto credentialsDto);
	}
}
