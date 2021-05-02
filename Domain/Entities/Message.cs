using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
	public class Message : IEntity
	{
		public long Id { get; set; }

		public int SenderId { get; set; }
		public User Sender { get; set; }

		public long ConversationId { get; set; }
		public Conversation Conversation { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime DeletedAt { get; set; }

		public List<Attachment> Attachments { get; set; }
	}
}
