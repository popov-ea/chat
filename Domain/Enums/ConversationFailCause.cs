using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
	public enum ConversationFailCause
	{
		/// <summary>
		/// No users in collection with invited users
		/// </summary>
		NoUsersInvited,
		/// <summary>
		/// One or more users has blocked current user
		/// </summary>
		BlockedUser,
	}
}
