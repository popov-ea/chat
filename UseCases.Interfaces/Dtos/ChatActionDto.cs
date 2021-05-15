using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Dtos
{
	public class ChatActionDto : IEntityDto
	{
		public long Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public ChatActionType Type { get; set; }

		public long ConversationId { get; set; }
		public ConversationDto Conversation { get; set; }

		public long InitiatorId { get; set; }
		public UserDto Initiator { get; set; }

		public long ActionTargetId { get; set; }
		public UserDto ActionTarget { get; set; }
	}
}
