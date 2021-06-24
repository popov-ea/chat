using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Dtos
{
	public class UserDto : IEntityDto
	{
		public long Id { get; set; }
		public string UserName { get; set; }

		public List<ConversationUserDto> Conversations { get; set; }
	}
}
