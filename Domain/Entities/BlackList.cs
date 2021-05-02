using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
	public class BlackList : IEntity
	{
		public long InitiatorId { get; set; }
		public User Initiator { get; set; }

		public long BlockedId { get; set; }
		public User Blocked { get; set; }
	}
}
