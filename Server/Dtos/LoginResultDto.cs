using System;
using System.Collections.Generic;
using System.Text;
using UseCases.Interfaces.Dtos;

namespace Server.Dtos
{
	public class LoginResultDto
	{
		public long UserId { get; set; }
		public string UserName { get; set; }
		public string Token { get; set; }
	}
}
