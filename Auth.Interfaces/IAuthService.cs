using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Interfaces
{
	public interface IAuthService
	{
		public Task<AuthResult> Authenticate(string login, string password);

		public Task<AuthResult> Register(string login, string password);
	}
}
