using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Server.Identity
{
	public class UserIdentity : IIdentity
	{
		private readonly string _authType;
		public string AuthenticationType { get => _authType; }

		private readonly bool _isAuthenticated;
		public bool IsAuthenticated { get => _isAuthenticated; }

		private readonly string _name;
		public string Name { get => _name; }

		public long UserId { get; set; }

		public UserIdentity(string name, bool isAuthenticated, string authType)
		{
			_name = name;
			_isAuthenticated = isAuthenticated;
			_authType = authType;
		}
	}
}
