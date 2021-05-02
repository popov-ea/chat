using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
	public enum BlackListFailCauses
	{
		/// <summary>
		/// User is already blocked by another user
		/// </summary>
		AlreadyBlocked,
		/// <summary>
		/// User is not blocked
		/// </summary>
		NotBlocked
	}
}
