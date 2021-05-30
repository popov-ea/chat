using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;

namespace UseCases.Interfaces.Services
{
	public interface IConversationService
	{
		public Task<ConversationServiceResultDto> CreateConversationAsync(long initiatorId, IEnumerable<long> invitedUserIds, string name = null);
		public Task<ConversationServiceResultDto> DeleteConversationAsync(long actorId, long conversationId);
		public Task<ConversationServiceResultDto> ClearConversationAsync(long actorId, long conversationID);
		public Task<ConversationDto> GetConversationAsync(long conversationId);
		public Task<IEnumerable<ConversationDto>> GetAllConversationsAsync();
		public Task<ConversationServiceResultDto> UpdateConversationAsync(long conversationId, ConversationDto newData);
	}
}
