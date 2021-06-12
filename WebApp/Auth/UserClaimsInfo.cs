using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Auth
{
	public class UserClaimsInfo
	{
		public string Login { get; }
		public long Id { get; }

		public UserClaimsInfo(long id, string login)
		{
			Id = id;
			Login = login;
		}
	}
}
