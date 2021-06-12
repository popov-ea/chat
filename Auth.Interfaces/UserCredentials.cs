using Domain.Entities;
using System;

namespace Auth.Interfaces
{
	public class UserCredentials
	{
		public long Id { get; set; }

		public long UserId { get; set; }
		public User User { get; set; }

		public string Login { get; set; }
		public string Password { get; set; }
	}
}
