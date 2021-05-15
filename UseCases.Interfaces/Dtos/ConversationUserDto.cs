using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Dtos
{
	public class ConversationUserDto : IEntityDto
	{
		public long Id { get; set; }
		public ConversationDto Conversation { get; set; }
		public long ConversationId { get; set; }

		public UserDto User { get; set; }
		public long UserId { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime DeletedAt { get; set; }
	}
}
