using Server.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UseCases.Interfaces.Dtos;

namespace Server.Dtos
{
	public class RegisterDto
	{
		public string UserName { get; set; }

		public string Login { get; set; }
		public string Password { get; set; }
	}
}
