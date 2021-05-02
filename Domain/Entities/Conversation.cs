using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
	public class Conversation : IEntity
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public long? OwnerId { get; set; }
		public User Owner { get; set; }

		public List<ConversationUser> Users { get; set; }
		public List<Message> Messages { get; set; }
		public List<ChatAction> ChatActions { get; set; }
	}
}
