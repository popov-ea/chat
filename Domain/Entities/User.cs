using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
	public class User : IEntity
	{
		public long Id { get; set; }
		public string Username { get; set; }

		public List<ConversationUser> Conversations { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime DeletedAt { get; set; }
	}
}
