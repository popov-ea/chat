using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Dtos
{
	class CreateConversationDto
	{
		public IEnumerable<long> InvitedUserIds { get; set; }
		public long InitiatorId { get; set; }
	}
}
