using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Dtos
{
	public class BlackListDto : IEntityDto
	{
		public long Id { get; set; }

		public long InitiatorId { get; set; }
		public UserDto Initiator { get; set; }

		public long BlockedId { get; set; }
		public UserDto Blocked { get; set; }
	}
}
