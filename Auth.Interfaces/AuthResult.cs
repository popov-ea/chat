using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Interfaces
{
	public class AuthResult
	{
		public bool Succeed { get; set; }
		public UserCredentialsDto Credentials { get; set; }
	}
}
