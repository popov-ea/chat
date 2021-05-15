using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Dtos
{
	public class MessageDto : IEntityDto
	{
		public long Id { get; set; }

		public string Text { get; set; }

		public long SenderId { get; set; }
		public UserDto Sender { get; set; }

		public long ConversationId { get; set; }
		public ConversationDto Conversation { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime DeletedAt { get; set; }

		public List<AttachmentDto> Attachments { get; set; }
	}
}
