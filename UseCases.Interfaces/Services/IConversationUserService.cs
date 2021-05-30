using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos.Results;

namespace UseCases.Interfaces.Services
{
	public interface IConversationUserService
	{
		public Task<ConversationUserServiceResultDto> AddUserAsync(long conversationId, long userId);
		public Task<ConversationUserServiceResultDto> DeleteUserAsync(long conversationId, long userId);
		public Task<ConversationUserServiceResultDto> DeleteUserAsync(long conversationUserId);
	}
}
