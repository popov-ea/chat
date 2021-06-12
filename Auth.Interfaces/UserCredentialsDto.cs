using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Interfaces
{
	public class UserCredentialsDto
	{
		public long Id { get; set; }

		public long UserId { get; set; }

		public string Login { get; set; }
		public string Password { get; set; }
	}
}
