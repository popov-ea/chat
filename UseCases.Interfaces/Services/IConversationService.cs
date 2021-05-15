using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;

namespace UseCases.Interfaces.Services
{
	public interface IConversationService
	{
		public Task<ConversationServiceResultDto> CreateConversationAsync(UserDto initiator, IEnumerable<UserDto> invitedUsers, string name = null);
		public Task<ConversationServiceResultDto> DeleteConversationAsync(UserDto actor, ConversationDto conversation);
		public Task<ConversationServiceResultDto> ClearConversationAsync(UserDto actor, ConversationDto conversation);
	}
}
