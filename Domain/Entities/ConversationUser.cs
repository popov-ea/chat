using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
	public class ConversationUser : IEntity
	{
		public long Id { get; set; }
		public Conversation Conversation { get; set; }
		public long ConversationId { get; set; }

		public User User { get; set; }
		public long UserId { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime DeletedAt { get; set; }
	}
}
