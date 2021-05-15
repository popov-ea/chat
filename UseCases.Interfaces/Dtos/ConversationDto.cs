using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Dtos
{
	public class ConversationDto : IEntityDto
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public long? OwnerId { get; set; }
		public UserDto Owner { get; set; }

		public List<ConversationUserDto> Users { get; set; }
		public List<MessageDto> Messages { get; set; }
		public List<ChatActionDto> ChatActions { get; set; }
	}
}
