using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
	public class ChatAction : IEntity
	{
		public long Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public ChatActionType Type { get; set; }

		public long ConversationId { get; set; }
		public Conversation Conversation { get; set; }

		public long InitiatorId { get; set; }
		public User Initiator { get; set; }

		public long ActionTargetId { get; set; }
		public User ActionTarget { get; set; }
	}
}
